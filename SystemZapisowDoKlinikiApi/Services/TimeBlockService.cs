using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.DTO.TimeBlocksDtos;
using SystemZapisowDoKlinikiApi.Exceptions;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

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
        var timeBlocks = await _timeBlockRepository.GetTimeBlocksAsync(id, date);
        if (timeBlocks.IsNullOrEmpty())
        {
            throw new KeyNotFoundException("No time blocks found for the given doctor and date.");
        }

        return timeBlocks;
    }

    public async Task<TimeBlockDto?> GetTimeBlockByDoctorBlockIdAsync(int id)
    {
        return await _timeBlockRepository.GetTimeBlockByDoctorBlockIdAsync(id);
    }

    public async Task<bool> CheckIfAvailableTimeBlockAsync(DateTime startTime, int duration)
    {
        return await _timeBlockRepository.CheckIfAvailableTimeBlockAsync(startTime, duration);
    }

    public async Task<List<WorkingHoursDto>> GetWorkingHoursAsync(int doctorId, DateTime date)
    {
        var (monday, sunday) = CalculateWeekRange(date);
        return await _timeBlockRepository.GetWorkingHoursAsync(doctorId, monday, sunday);
    }

    private (DateTime Monday, DateTime Sunday) CalculateWeekRange(DateTime date)
    {
        int daysFromMonday = date.DayOfWeek == DayOfWeek.Sunday
            ? 6
            : (int)date.DayOfWeek - 1;

        var monday = date.AddDays(-daysFromMonday);
        var sunday = monday.AddDays(6);
        return (monday, sunday);
    }

    public async Task DeleteWorkingHoursAsync(int doctorId, WorkingHoursDto workingHoursDto)
    {
        ValidateWorkingHoursDeletion(workingHoursDto);

        await _timeBlockRepository.DeleteWorkingHoursAsync(doctorId, workingHoursDto);
    }

    private void ValidateWorkingHoursDeletion(WorkingHoursDto workingHoursDto)
    {
        if (workingHoursDto.StartTime < DateTime.Now)
        {
            throw new BusinessException("INVALID_WORKING_HOURS_DELETION",
                "Cannot delete working hours for today or past dates.");
        }
    }

    public async Task AddWorkingHoursAsync(int doctorId, WorkingHoursDto workingHoursDto)
    {
        ValidateWorkingHoursAddition(workingHoursDto);

        await _timeBlockRepository.AddWorkingHoursAsync(doctorId, workingHoursDto);
    }

    private void ValidateWorkingHoursAddition(WorkingHoursDto workingHoursDto)
    {
        if (workingHoursDto.StartTime < DateTime.Now)
        {
            throw new BusinessException("INVALID_WORKING_HOURS_ADDITION",
                "Cannot add working hours for today or past dates.");
        }
    }
}