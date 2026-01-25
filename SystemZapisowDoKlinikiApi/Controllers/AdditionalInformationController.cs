using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.Attributes;
using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/additional-information")]
public class AdditionalInformationController : ControllerBase
{
    private readonly IAdditionalInformationService _additionalInformationService;
    private readonly ILogger<AdditionalInformationController> _logger;

    public AdditionalInformationController(IAdditionalInformationService additionalInformationService,
        ILogger<AdditionalInformationController> logger)
    {
        _additionalInformationService = additionalInformationService;
        _logger = logger;
    }

    [HttpPost("")]
    [Authorize(Roles = "Doctor")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> CreateAddInformationAsync([FromBody] AddInformationDto addInformationDto)
    {
        _logger.LogInformation("Creating additional information entry by doctor.");

        var newAddInfo = await _additionalInformationService.CreateAddInformationAsync(addInformationDto);

        return CreatedAtAction(
            "GetAddInformationById",
            new { id = newAddInfo.Id },
            newAddInfo);
    }

    [HttpGet("")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetAddInformationAsync([FromQuery] string lang)
    {
        _logger.LogInformation("Retrieving additional information entries for doctor with language preference: {Lang}",
            lang);
        var additionalInfo = await _additionalInformationService.GetAddInformationAsync(lang);
        return Ok(additionalInfo);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetAddInformationByIdAsync([FromRoute] int id, string lang)
    {
        _logger.LogInformation("Retrieving additional information entry with id: {Id}", id);
        var additionalInfo = await _additionalInformationService.GetAddInformationByIdAsync(id, lang);
        return Ok(additionalInfo);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Doctor")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> DeleteAddInformationByIdAsync([FromRoute] int id)
    {
        _logger.LogInformation("Deleting additional information entry with id: {Id}", id);
        await _additionalInformationService.DeleteAddInformationByIdAsync(id);
        return NoContent();
    }
}