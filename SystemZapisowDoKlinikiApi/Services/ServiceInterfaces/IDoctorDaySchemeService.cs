using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IDoctorDaySchemeService
{
    public Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDto weekSchemeDto);
    public Task<WeekSchemeDto> GetDoctorWeekSchemeAsync(int userId);
}