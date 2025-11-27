using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IDoctorDaySchemeRepository
{
    public Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDTO weekSchemeDto);
    Task<WeekSchemeDTO> GetDoctorWeekSchemeAsync(int UserId);
}