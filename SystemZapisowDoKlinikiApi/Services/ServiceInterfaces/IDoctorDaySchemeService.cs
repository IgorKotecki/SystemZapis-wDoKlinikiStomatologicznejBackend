using SystemZapisowDoKlinikiApi.DTO.DaySchemeDtos;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface IDoctorDaySchemeService
{
    public Task UpdateDoctorWeekSchemeAsync(int doctorId, WeekSchemeDto weekSchemeDto);
    public Task<WeekSchemeDto> GetDoctorWeekSchemeAsync(int userId);
    Task<DateTime> GetNextScheduledDayAsync(int doctorId);
}