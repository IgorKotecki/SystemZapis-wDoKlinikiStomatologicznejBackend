using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToothController : ControllerBase
{
    private readonly IToothService _toothService;

    public ToothController(IToothService toothService)
    {
        _toothService = toothService;
    }

    [HttpPost]
    [Route("ToothModel")]
    public async Task<ActionResult<ToothModelOutDTO>> GetToothModelAsync([FromBody] ToothModelRequest request)
    {
        try
        {
            var toothModel = await _toothService.GetToothModelAsync(request);
            return Ok(toothModel);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut]
    [Route("UpdateToothModel")]
    public async Task<IActionResult> UpdateToothModelAsync([FromBody] ToothModelInDTO request)
    {
        try
        {
            await _toothService.UpdateToothModelAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("Statuses")]
    public async Task<ActionResult<ToothStatusesDto>> GetToothStatuses([FromQuery] string language)
    {
        try
        {
            var statuses = await _toothService.GetToothStatusesAsync(language);
            return Ok(statuses);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}