using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IDoctorDaySchemeRepository
{
    public Task UpdateDoctorDaySchemeAsync(int doctorId, int dayOfWeek, DaySchemeDto daySchemeDto);
}