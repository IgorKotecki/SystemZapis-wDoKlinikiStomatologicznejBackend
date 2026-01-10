using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Exceptions;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class TimeBlockRepository : ITimeBlockRepository
{
    private readonly ClinicDbContext _context;

    public TimeBlockRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(int id, DateRequest date)
    {
        var startOfDay = new DateTime(date.Year, date.Month, date.Day);
        var endOfDay = startOfDay.AddDays(1);

        var doctorBlocks = await _context.DoctorBlocks
            .Where(db => db.TimeBlock.TimeStart >= startOfDay && db.TimeBlock.TimeStart < endOfDay &&
                         db.DoctorUser.UserId == id)
            .Select(db => new TimeBlockDto
            {
                DoctorBlockId = db.Id,
                TimeStart = db.TimeBlock.TimeStart,
                TimeEnd = db.TimeBlock.TimeEnd,
                User = new UserDTO()
                {
                    Id = db.DoctorUser.UserId,
                    Name = db.DoctorUser.User.Name,
                    Surname = db.DoctorUser.User.Surname,
                    Email = db.DoctorUser.User.Email,
                    PhoneNumber = db.DoctorUser.User.PhoneNumber
                },
                IsAvailable = db.Appointments.All(a => a.DoctorBlockId != db.Id),
            })
            .ToListAsync();

        return doctorBlocks;
    }

    public async Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id)
    {
        return await _context.DoctorBlocks.Where(db => db.Id == id).Select(db => new TimeBlockDto()
        {
            DoctorBlockId = db.Id,
            TimeStart = db.TimeBlock.TimeStart,
            TimeEnd = db.TimeBlock.TimeEnd,
            User = new UserDTO()
            {
                Id = db.DoctorUser.UserId,
                Name = db.DoctorUser.User.Name,
                Surname = db.DoctorUser.User.Surname,
                Email = db.DoctorUser.User.Email,
                PhoneNumber = db.DoctorUser.User.PhoneNumber
            },
            IsAvailable = db.Appointments.All(a => a.DoctorBlockId != db.Id),
        }).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckIfAvailableTimeBlockAsync(DateTime startTime, int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            var time = startTime.AddMinutes(30 * i);
            var occupiedDoctorBlock =
                await _context.Appointments.AnyAsync(db => db.DoctorBlock.TimeBlock.TimeStart == time);
            if (occupiedDoctorBlock)
            {
                throw new Exception("Cannot book appointment, this hour is occupied");
            }
        }

        return true;
    }

    public async Task<List<WorkingHoursDto>> GetWorkingHoursAsync(int doctorId, DateTime monday, DateTime sunday)
    {
        var blocks = await _context.DoctorBlocks
            .Where(db => db.TimeBlock.TimeStart >= monday.Date &&
                         db.TimeBlock.TimeStart <= sunday.Date &&
                         db.DoctorUser.UserId == doctorId)
            .Select(db => new WorkingHoursDto()
            {
                StartTime = db.TimeBlock.TimeStart,
                EndTime = db.TimeBlock.TimeEnd
            })
            .OrderBy(b => b.StartTime)
            .ToListAsync();

        return MergeContinuousBlocks(blocks);
    }

    public async Task DeleteWorkingHoursAsync(int doctorId, DateTime date)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var hasAppointments = await _context.DoctorBlocks
                .Where(db =>
                    db.DoctorUser.UserId == doctorId &&
                    db.TimeBlock.TimeStart.Date == date.Date)
                .AnyAsync(db => db.Appointments.Any());

            if (hasAppointments)
            {
                throw new BusinessException("DAY_HAS_APPOINTMENTS",
                    "Cannot delete working hours for the day that has appointments.");
            }

            var blocksToDelete = await _context.DoctorBlocks
                .Where(db =>
                    db.DoctorUser.UserId == doctorId &&
                    db.TimeBlock.TimeStart.Date == date.Date)
                .ToListAsync();

            _context.DoctorBlocks.RemoveRange(blocksToDelete);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task AddWorkingHoursAsync(int doctorId, WorkingHoursDto workingHoursDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var existingBlocks = await _context.DoctorBlocks
                .Where(db =>
                    db.DoctorUser.UserId == doctorId &&
                    db.TimeBlock.TimeStart.Date == workingHoursDto.StartTime.Date)
                .AnyAsync();
            if (existingBlocks)
            {
                throw new BusinessException("WORKING_HOURS_ALREADY_EXIST",
                    "Working hours for the specified day already exist.");
            }

            var timeBlocks = await _context.TimeBlocks
                .Where(tb => tb.TimeStart >= workingHoursDto.StartTime && tb.TimeEnd <= workingHoursDto.EndTime)
                .ToListAsync();
            foreach (var timeBlock in timeBlocks)
            {
                _context.DoctorBlocks.Add(new DoctorBlock
                {
                    DoctorUserId = doctorId,
                    TimeBlockId = timeBlock.Id
                });
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private List<WorkingHoursDto> MergeContinuousBlocks(List<WorkingHoursDto> blocks)
    {
        var result = new List<WorkingHoursDto>();

        foreach (var dayGroup in blocks.GroupBy(b => b.StartTime.Date))
        {
            var sortedBlocks = dayGroup.OrderBy(b => b.StartTime).ToList();

            if (!sortedBlocks.Any()) continue;

            var current = new WorkingHoursDto
            {
                StartTime = sortedBlocks[0].StartTime,
                EndTime = sortedBlocks[0].EndTime
            };

            for (int i = 1; i < sortedBlocks.Count; i++)
            {
                if (sortedBlocks[i].StartTime == current.EndTime)
                {
                    current.EndTime = sortedBlocks[i].EndTime;
                }
                else
                {
                    result.Add(current);
                    current = new WorkingHoursDto
                    {
                        StartTime = sortedBlocks[i].StartTime,
                        EndTime = sortedBlocks[i].EndTime
                    };
                }
            }

            result.Add(current);
        }

        return result.OrderBy(r => r.StartTime).ToList();
    }
}