using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IDoctorDaySchemeService
{
    public Task UpdateDoctorDaySchemeAsync(int doctorId, int dayOfWeek, DaySchemeDto daySchemeDto);
}