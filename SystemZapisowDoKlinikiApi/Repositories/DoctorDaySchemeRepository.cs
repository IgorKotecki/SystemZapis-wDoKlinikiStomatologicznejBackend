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

    public async Task UpdateDoctorDaySchemeAsync(int doctorId, int dayOfWeek, DaySchemeDto daySchemeDto)
    {
        var existingScheme = await _context.DaySchemeTimeBlocks.FirstOrDefaultAsync(d =>
            d.DoctorUser.UserId == doctorId && d.WeekDay == dayOfWeek);

        if (existingScheme != null)
        {
            existingScheme.JobStart = daySchemeDto.StartHour;
            existingScheme.JobEnd = daySchemeDto.EndHour;
            _context.DaySchemeTimeBlocks.Update(existingScheme);
        }
        else
        {
            var doctorUser = await _context.Doctors.FirstOrDefaultAsync(du => du.UserId == doctorId);
            if (doctorUser == null)
            {
                throw new ArgumentException($"Doctor with ID {doctorId} not found.");
            }
            var newScheme = new DaySchemeTimeBlock
            {
                DoctorUser = doctorUser,
                WeekDay = dayOfWeek,
                JobStart = daySchemeDto.StartHour,
                JobEnd = daySchemeDto.EndHour,
            };
            _context.DaySchemeTimeBlocks.Add(newScheme);
        }

        await _context.SaveChangesAsync();
    }
}