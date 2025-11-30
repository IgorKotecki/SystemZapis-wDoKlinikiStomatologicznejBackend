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

    public async Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(int id, DateRequest date)
    {
        return await _timeBlockRepository.GetTimeBlocksAsync(id, date);
    }

    public async Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id)
    {
        return await _timeBlockRepository.GetTimeBlockByDoctorBlockIdAsync(id);
    }

    public async Task<bool> CheckIfAvailableTimeBlockAsync(DateTime startTime, int duration)
    {
        return await _timeBlockRepository.CheckIfAvailableTimeBlockAsync(startTime, duration);
    }
}