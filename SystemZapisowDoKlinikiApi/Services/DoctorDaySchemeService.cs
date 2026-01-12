using SystemZapisowDoKlinikiApi.DTO.DaySchemeDtos;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Services;

public class DoctorDaySchemeService : IDoctorDaySchemeService
{
    private readonly IDoctorDaySchemeRepository _doctorDaySchemeRepository;

    public DoctorDaySchemeService(IDoctorDaySchemeRepository doctorDaySchemeRepository)
    {
        _doctorDaySchemeRepository = doctorDaySchemeRepository;
    }

    public async Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDto weekSchemeDto)
    {
        await _doctorDaySchemeRepository.UpdateDoctorWeekSchemeAsync(doctorId, weekSchemeDto);
    }

    public async Task<WeekSchemeDto> GetDoctorWeekSchemeAsync(int userId)
    {
        return await _doctorDaySchemeRepository.GetDoctorWeekSchemeAsync(userId);
    }

    public async Task<DateTime> GetNextScheduledDayAsync(int doctorId)
    {
        return await _doctorDaySchemeRepository.GetNextScheduledDayAsync(doctorId);
    }
}