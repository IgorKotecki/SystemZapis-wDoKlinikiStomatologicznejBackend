using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class DoctorDaySchemeRepository : IDoctorDaySchemeRepository
{
    private readonly ClinicDbContext _context;

    public DoctorDaySchemeRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDTO weekSchemeDto)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            for (int i = 1; i < 7; i++)
            {
                var daySchemeDto = weekSchemeDto.DaysSchemes.FirstOrDefault(ds => ds.DayOfWeek == i);
                if (daySchemeDto == null)
                {
                    var weekDay = i;
                    await _context.DaySchemeTimeBlocks
                        .Where(ds => ds.DoctorUser.UserId == doctorId && ds.WeekDay == weekDay)
                        .ExecuteDeleteAsync();
                    continue;
                }

                var existingDayScheme = await _context.DaySchemeTimeBlocks
                    .FirstOrDefaultAsync(ds =>
                        ds.DoctorUser.UserId == doctorId && ds.WeekDay == daySchemeDto.DayOfWeek);
                if (existingDayScheme != null)
                {
                    existingDayScheme.JobStart = daySchemeDto.StartHour;
                    existingDayScheme.JobEnd = daySchemeDto.EndHour;
                    _context.DaySchemeTimeBlocks.Update(existingDayScheme);
                }
                else
                {
                    var doctorUser = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == doctorId);
                    if (doctorUser == null)
                        throw new Exception("Doctor not found");

                    var newDayScheme = new DaySchemeTimeBlock
                    {
                        DoctorUser = doctorUser,
                        WeekDay = daySchemeDto.DayOfWeek,
                        JobStart = daySchemeDto.StartHour,
                        JobEnd = daySchemeDto.EndHour
                    };
                    await _context.DaySchemeTimeBlocks.AddAsync(newDayScheme);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<WeekSchemeDTO> GetDoctorWeekSchemeAsync(int UserId)
    {
        var weekScheme = new WeekSchemeDTO()
        {
            DaysSchemes = await _context.DaySchemeTimeBlocks
                .Where(d => d.DoctorUser.UserId == UserId)
                .Select(ds => new DaySchemeDto()
                {
                    DayOfWeek = ds.WeekDay,
                    StartHour = ds.JobStart,
                    EndHour = ds.JobEnd
                }).ToListAsync()
        };
        return weekScheme;
    }
}