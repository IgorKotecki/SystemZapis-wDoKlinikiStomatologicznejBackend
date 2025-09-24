using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface ITimeBlockService
{
    Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(DateRequest date);
    Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id);
}