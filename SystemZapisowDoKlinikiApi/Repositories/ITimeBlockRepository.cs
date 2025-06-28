using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface ITimeBlockRepository
{
    public Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(DateRequest date);
}