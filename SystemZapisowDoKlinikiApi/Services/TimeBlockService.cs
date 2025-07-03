using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class TimeBlockService : ITimeBlockService
{
    private readonly ITimeBlockRepository _timeBlockRepository;

    public TimeBlockService(ITimeBlockRepository timeBlockRepository)
    {
        _timeBlockRepository = timeBlockRepository;
    }

    public async Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(DateRequest date)
    {
        return await _timeBlockRepository.GetTimeBlocksAsync(date);
    }

    public async Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id)
    {
        return await _timeBlockRepository.GetTimeBlockByDoctorBlockIdAsync(id);
    }
}