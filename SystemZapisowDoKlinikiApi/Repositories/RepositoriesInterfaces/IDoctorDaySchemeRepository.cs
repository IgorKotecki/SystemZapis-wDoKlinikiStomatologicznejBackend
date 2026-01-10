using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IDoctorDaySchemeRepository
{
    public Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDto weekSchemeDto);
    Task<WeekSchemeDto> GetDoctorWeekSchemeAsync(int UserId);
}