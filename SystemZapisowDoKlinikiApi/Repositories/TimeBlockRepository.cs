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

    public async Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(DateRequest date)
    {
        var startOfDay = new DateTime(date.Year, date.Month, date.Day);
        var endOfDay = startOfDay.AddDays(1);
        Console.WriteLine(date);

        var doctorBlocks = await _context.DoctorBlocks
            .Where(db => db.TimeBlock.TimeStart >= startOfDay && db.TimeBlock.TimeStart < endOfDay)
            .Select(db => new TimeBlockDto
            {
                DoctorBlockId = db.Id,
                TimeStart = db.TimeBlock.TimeStart,
                TimeEnd = db.TimeBlock.TimeEnd,
                UserId = db.DoctorUserId,
                isAvailable = db.Appointments.All(a => a.DoctorBlockId != db.Id),
            })
            .ToListAsync();

        return doctorBlocks;
    }
}