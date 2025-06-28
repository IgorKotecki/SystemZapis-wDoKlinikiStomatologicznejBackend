using Microsoft.AspNetCore.Http.HttpResults;
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
}