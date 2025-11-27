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

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet("UserServices")]
    public async Task<ActionResult<ICollection<ServiceDTO>>> GetAllServicesAvailableForClientWithLangAsync(
        [FromQuery] string lang)
    {
        try
        {
            var services = await _serviceService.GetAllServicesAvailableForClientWithLangAsync(lang);
            return Ok(services);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("addService")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddServiceAsync([FromBody] AddServiceDto addServiceDto)
    {
        try
        {
            await _serviceService.AddServiceAsync(addServiceDto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("AllServices")]
    public async Task<IActionResult> GetAllServicesAsync([FromQuery] string lang)
    {
        try
        {
            var services = await _serviceService.GerAllServicesAsync(lang);
            if (!services.Any())
                return NotFound("No services found.");
            return Ok(services);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("deleteService/{serviceId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteServiceAsync([FromRoute] int serviceId)
    {
        try
        {
            await _serviceService.DeleteServiceAsync(serviceId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(e.Message);
        }
    }
}