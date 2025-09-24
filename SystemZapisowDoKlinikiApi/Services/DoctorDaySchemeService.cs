using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class DoctorDaySchemeService : IDoctorDaySchemeService
{
    private readonly IDoctorDaySchemeRepository _DoctorDaySchemeRepository;

    public DoctorDaySchemeService(IDoctorDaySchemeRepository doctorDaySchemeRepository)
    {
        _DoctorDaySchemeRepository = doctorDaySchemeRepository;
    }

    public async Task UpdateDoctorDaySchemeAsync(int doctorId, int dayOfWeek, DaySchemeDto daySchemeDto)
    {
        if (IsValidTimeRange(daySchemeDto.StartHour, daySchemeDto.EndHour) && IsValidDayOfWeek(dayOfWeek))
        {
            await _DoctorDaySchemeRepository.UpdateDoctorDaySchemeAsync(doctorId, dayOfWeek, daySchemeDto);
        }
        else
        {
            throw new ArgumentException("Invalid time range or day of the week.");
        }
    }

    private bool IsValidDayOfWeek(int dayOfWeek)
    {
        return dayOfWeek >= 0 && dayOfWeek <= 6;
    }

    private bool IsValidTimeRange(TimeOnly start, TimeOnly end)
    {
        return start < end && start >= new TimeOnly(8, 0, 0) && end <= new TimeOnly(20, 0, 0);
    }
}