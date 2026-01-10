using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorDaySchemeService _doctorDaySchemeService;
    private readonly IDoctorService _doctorService;
    private readonly ILogger<DoctorController> _logger;

    public DoctorController(IDoctorDaySchemeService doctorDaySchemeService, IDoctorService doctorService,
        ILogger<DoctorController> logger)
    {
        _doctorDaySchemeService = doctorDaySchemeService;
        _doctorService = doctorService;
        _logger = logger;
    }

    [HttpPut]
    [Route("week-scheme")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> UpdateDoctorDaySchemeAsync([FromBody] WeekSchemeDto weekSchemeDto)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        await _doctorDaySchemeService.UpdateDoctorWeekSchemeAsync(int.Parse(userId), weekSchemeDto);

        _logger.LogInformation("Doctor with id: {userId} updated their week scheme.", userId);

        return Ok();
    }

    [HttpGet]
    [Route("week-scheme")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorWeekSchemeAsync()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var weekScheme = await _doctorDaySchemeService.GetDoctorWeekSchemeAsync(int.Parse(userId));

        _logger.LogInformation("Doctor with id: {userId} retrieved their week scheme.", userId);

        return Ok(weekScheme);
    }

    [HttpPost]
    [Route("doctor")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDoctorAsync([FromBody] AddDoctorDto addDoctorDto)
    {
        await _doctorService.AddDoctorAsync(addDoctorDto);

        _logger.LogInformation("New doctor added for user with ID: {userId}", addDoctorDto.UserId);

        return Ok();
    }

    [HttpDelete]
    [Route("doctor/{doctorId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDoctorAsync(int doctorId)
    {
        await _doctorService.DeleteDoctorAsync(doctorId);

        _logger.LogInformation("Doctor with ID: {doctorId} has been deleted.", doctorId);

        return Ok();
    }

    [HttpGet]
    //JEZELI BEDZIESZ TU DODWAŁ COS TO ROLA DOCTOR I RECEPtionist musza byc 
    public async Task<IActionResult> GetDoctorsAsync()
    {
        var result = await _doctorService.GetDoctorsAsync();

        _logger.LogInformation("Retrieved list of doctors.");

        return Ok(result);
    }
}