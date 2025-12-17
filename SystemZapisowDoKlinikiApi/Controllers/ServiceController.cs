using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ILogger<ServiceController> _logger;

    public ServiceController(IServiceService serviceService, ILogger<ServiceController> logger)
    {
        _serviceService = serviceService;
        _logger = logger;
    }

    [HttpGet("user-services")]
    public async Task<ActionResult<ICollection<ServiceDTO>>> GetAllServicesAvailableForClientWithLangAsync(
        [FromQuery] string lang)
    {
        var services = await _serviceService.GetAllServicesAvailableForClientWithLangAsync(lang);

        _logger.LogInformation("Retrieved {Count} services for client with language preference: {Lang}", services.Count,
            lang);

        return Ok(services);
    }

    [HttpPost]
    [Route("service")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddServiceAsync([FromBody] AddServiceDto addServiceDto)
    {
        await _serviceService.AddServiceAsync(addServiceDto);

        _logger.LogInformation("Added new service: {ServiceName}", addServiceDto.Languages.FirstOrDefault()?.Name);

        return Ok();
    }

    [HttpGet("services")]
    public async Task<IActionResult> GetAllServicesAsync([FromQuery] string lang)
    {
        var services = await _serviceService.GerAllServicesAsync(lang);

        _logger.LogInformation("Retrieved all services by categories for language: {Lang}", lang);

        return Ok(services);
    }

    [HttpDelete]
    [Route("service/{serviceId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteServiceAsync([FromRoute] int serviceId)
    {
        await _serviceService.DeleteServiceAsync(serviceId);

        _logger.LogInformation("Deleted service with ID: {ServiceId}", serviceId);

        return Ok();
    }
}