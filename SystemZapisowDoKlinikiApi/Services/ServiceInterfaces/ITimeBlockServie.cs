using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface ITimeBlockService
{
    Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(int id, DateRequest date);
    Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id);
    Task<bool> CheckIfAvailableTimeBlockAsync(DateTime startTime, int duration);
}