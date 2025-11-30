using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.DTO;
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
        Console.WriteLine(date);

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
                isAvailable = db.Appointments.All(a => a.DoctorBlockId != db.Id),
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
            isAvailable = db.Appointments.All(a => a.DoctorBlockId != db.Id)
            //Troche ciezka operacja mozna od razu przy zapisywawniu wstawiac flage dostepnosci
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
}