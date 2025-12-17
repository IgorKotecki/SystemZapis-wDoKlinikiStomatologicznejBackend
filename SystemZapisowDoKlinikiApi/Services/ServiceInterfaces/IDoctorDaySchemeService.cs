using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IDoctorDaySchemeService
{
    public Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDTO weekSchemeDto);
    public Task<WeekSchemeDTO> GetDoctorWeekSchemeAsync(int userId);
}