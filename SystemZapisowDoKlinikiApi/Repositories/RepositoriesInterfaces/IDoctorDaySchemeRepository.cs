using SystemZapisowDoKlinikiApi.DTO.DaySchemeDtos;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface IDoctorDaySchemeRepository
{
    public Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDto weekSchemeDto);
    Task<WeekSchemeDto> GetDoctorWeekSchemeAsync(int userId);
    Task<DateTime> GetNextScheduledDayAsync(int doctorId);
}