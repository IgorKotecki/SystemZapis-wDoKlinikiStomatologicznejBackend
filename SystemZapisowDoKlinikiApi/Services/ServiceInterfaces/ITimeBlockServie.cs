using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface ITimeBlockService
{
    Task<ICollection<TimeBlockDto>> GetTimeBlocksAsync(int id, DateRequest date);
    Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id);
    Task<bool> CheckIfAvailableTimeBlockAsync(DateTime startTime, int duration);
    Task<List<WorkingHoursDto>> GetWorkingHoursAsync(int doctorId, DateTime date);
    Task DeleteWorkingHoursAsync(int doctorId, WorkingHoursDto workingHoursDto);
    Task AddWorkingHoursAsync(int doctorId, WorkingHoursDto workingHoursDto);
}