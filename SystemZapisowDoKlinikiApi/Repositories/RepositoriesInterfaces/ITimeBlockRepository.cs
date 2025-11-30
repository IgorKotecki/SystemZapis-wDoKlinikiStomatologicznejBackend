using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface ITimeBlockRepository
{
    public Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(int id, DateRequest date);
    public Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id);
    Task<bool> CheckIfAvailableTimeBlockAsync(DateTime startTime, int duration);
}