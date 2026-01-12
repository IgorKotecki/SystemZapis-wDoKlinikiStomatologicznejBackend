using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.DTO.TimeBlocksDtos;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface ITimeBlockRepository
{
    public Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(int id, DateRequest date);
    public Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id);
    Task<bool> CheckIfAvailableTimeBlockAsync(DateTime startTime, int duration);
    Task<List<WorkingHoursDto>> GetWorkingHoursAsync(int doctorId, DateTime monday, DateTime sunday);
    Task DeleteWorkingHoursAsync(int doctorId, WorkingHoursDto workingHoursDto);
    Task AddWorkingHoursAsync(int doctorId, WorkingHoursDto workingHoursDto);
}