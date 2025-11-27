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

    public async Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDTO weekSchemeDto)
    {
        await _DoctorDaySchemeRepository.UpdateDoctorWeekSchemeAsync(doctorId, weekSchemeDto);
    }

    public async Task<WeekSchemeDTO> GetDoctorWeekSchemeAsync(int userId)
    {
        return await _DoctorDaySchemeRepository.GetDoctorWeekSchemeAsync(userId);
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