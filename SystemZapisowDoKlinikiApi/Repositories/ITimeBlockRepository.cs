using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface ITimeBlockRepository
{
    public Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(DateRequest date);
    public Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id);
}