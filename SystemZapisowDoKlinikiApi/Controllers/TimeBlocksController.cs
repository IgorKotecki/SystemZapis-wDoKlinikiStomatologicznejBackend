using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeBlocksController : ControllerBase
{
    private readonly ITimeBlockService _timeBlockService;

    public TimeBlocksController(ITimeBlockService timeBlockService)
    {
        _timeBlockService = timeBlockService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTimeBlocks([FromQuery] DateRequest date)
    {
        var timeBlocks = await _timeBlockService.GetTimeBlocksAsync(date);
        return Ok(timeBlocks);
    }
}