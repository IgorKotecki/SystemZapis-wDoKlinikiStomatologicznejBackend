using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

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
    public async Task<ActionResult<ICollection<ServiceDto>>> GetAllServicesAvailableForClientWithLangAsync(
        [FromQuery] string lang)
    {
        var services = await _serviceService.GetAllServicesAvailableForClientWithLangAsync(lang);

        _logger.LogInformation("Retrieved {Count} services for client with language preference: {Lang}", services.Count,
            lang);

        return Ok(services);
    }

    [HttpPost]
    [Route("service")]
    [Authorize(Roles = "Doctor,Receptionist,Admin")]
    public async Task<IActionResult> AddServiceAsync([FromBody] AddServiceDto addServiceDto)
    {
        await _serviceService.AddServiceAsync(addServiceDto);

        _logger.LogInformation("Added new service: {ServiceName}", addServiceDto.Languages.FirstOrDefault()?.Name);

        return Ok();
    }

    [HttpGet("receptionist-services")]
    [Authorize(Roles = "Doctor,Receptionist,Admin")]
    public async Task<ActionResult<ICollection<ServiceDto>>> GetAllServicesForReceptionistAsync([FromQuery] string lang)
    {
        var services = await _serviceService.GetAllServicesForReceptionistAsync(lang);

        _logger.LogInformation("Retrieved all services for receptionist with language preference: {Lang}", lang);

        return Ok(services);
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
    [Authorize(Roles = "Admin,Receptionist,Doctor")]
    public async Task<IActionResult> DeleteServiceAsync([FromRoute] int serviceId)
    {
        await _serviceService.DeleteServiceAsync(serviceId);

        _logger.LogInformation("Deleted service with ID: {ServiceId}", serviceId);

        return Ok();
    }

    [HttpPut("editService/{serviceId}")]
    [Authorize(Roles = "Admin,Receptionist,Doctor")]
    public async Task<IActionResult> EditServiceAsync([FromRoute] int serviceId,
        [FromBody] ServiceEditDto serviceEditDto)
    {
        try
        {
            await _serviceService.EditServiceAsync(serviceId, serviceEditDto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("serviceCategories")]
    public async Task<IActionResult> GetAllServiceCategories()
    {
        var categories = await _serviceService.GetAllServiceCategories();
        return Ok(categories);
    }

    [HttpGet("{serviceId}")]
    public async Task<IActionResult> GetServiceById(int serviceId)
    {
        var service = await _serviceService.GetServiceByIdAsync(serviceId);

        if (service == null)
        {
            return NotFound($"Service with id = {serviceId} not found.");
        }

        return Ok(service);
    }

    [HttpGet("edit/{serviceId}")]
    [Authorize(Roles = "Admin,Receptionist,Doctor")]
    public async Task<IActionResult> GetServiceForEdit(int serviceId)
    {
        var service = await _serviceService.GetServiceForEditAsync(serviceId);

        if (service == null)
            return NotFound();

        return Ok(service);
    }

    [HttpPost]
    [Route("service/add")]
    [Authorize(Roles = "Doctor,Receptionist,Admin")]
    public async Task<IActionResult> AddServiceForTestAsync([FromBody] AddServiceDto addServiceDto)
    {
        await _serviceService.AddServiceAsync(addServiceDto);

        _logger.LogInformation("Added new service for test: {ServiceName}",
            addServiceDto.Languages.FirstOrDefault()?.Name);

        return Ok();
    }
}