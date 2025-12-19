using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToothController : ControllerBase
{
    private readonly IToothService _toothService;
    private readonly ILogger<ToothController> _logger;

    public ToothController(IToothService toothService, ILogger<ToothController> logger)
    {
        _toothService = toothService;
        _logger = logger;
    }

    [HttpGet]
    [Route("teeth-model")]
    public async Task<ActionResult<ToothModelOutDTO>> GetToothModelAsync([FromQuery] int userId,
        [FromQuery] string lang)
    {
        var request = new ToothModelRequest()
        {
            UserId = userId,
            Language = lang
        };
        var toothModel = await _toothService.GetToothModelAsync(request);

        _logger.LogInformation("Retrieved tooth model for user with id: {UserId}", userId);

        return Ok(toothModel);
    }

    [HttpPut]
    [Route("teeth-model")]
    public async Task<IActionResult> UpdateToothModelAsync([FromBody] ToothModelInDTO request)
    {
        await _toothService.UpdateToothModelAsync(request);

        _logger.LogInformation("Updated tooth model for user with id: {UserId}", request.UserId);

        return Ok();
    }

    [HttpGet]
    [Route("statuses")]
    public async Task<ActionResult<ToothStatusesDto>> GetToothStatuses([FromQuery] string lang)
    {
        var statuses = await _toothService.GetToothStatusesAsync(lang);

        _logger.LogInformation("Retrieved tooth statuses for language: {Lang}", lang);

        return Ok(statuses);
    }
}