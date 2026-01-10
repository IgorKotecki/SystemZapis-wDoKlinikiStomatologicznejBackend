using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api")]
public class TimeBlocksController : ControllerBase
{
    private readonly ITimeBlockService _timeBlockService;
    private readonly ILogger<TimeBlocksController> _logger;

    public TimeBlocksController(ITimeBlockService timeBlockService, ILogger<TimeBlocksController> logger)
    {
        _timeBlockService = timeBlockService;
        _logger = logger;
    }

    [HttpGet("time-blocks/{doctorId}")]
    public async Task<IActionResult> GetTimeBlocks(int doctorId, [FromQuery] DateRequest date)
    {
        var timeBlocks = await _timeBlockService.GetTimeBlocksAsync(doctorId, date);

        _logger.LogInformation("Retrieved {Count} time blocks for doctor with id: {DoctorId} on date: {Date}",
            timeBlocks.Count, doctorId, date.ToString());

        return Ok(timeBlocks);
    }

    [HttpGet("time-blocks/working-hours")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetWorkingHours([FromQuery] DateTime date)
    {
        var doctorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? throw new UnauthorizedAccessException("User ID not found in claims"));

        var workingHours = await _timeBlockService.GetWorkingHoursAsync(doctorId, date);

        _logger.LogInformation("Retrieved working hours for doctor with id: {DoctorId} on date: {Date}",
            doctorId, date.ToString());

        return Ok(workingHours);
    }
}