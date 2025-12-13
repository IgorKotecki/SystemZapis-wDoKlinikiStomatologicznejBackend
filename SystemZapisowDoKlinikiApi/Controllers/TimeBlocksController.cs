using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api")]
public class TimeBlocksController : ControllerBase
{
    private readonly ITimeBlockService _timeBlockService;

    public TimeBlocksController(ITimeBlockService timeBlockService)
    {
        _timeBlockService = timeBlockService;
    }

    [HttpGet("GetTimeBlocks/{id}")]
    public async Task<IActionResult> GetTimeBlocks(int id, [FromQuery] DateRequest date)
    {
        var timeBlocks = await _timeBlockService.GetTimeBlocksAsync(id, date);
        return Ok(timeBlocks);
    }
}