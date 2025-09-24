using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorDaySchemeController : ControllerBase
{
    private readonly IDoctorDaySchemeService _doctorDaySchemeService;

    public DoctorDaySchemeController(IDoctorDaySchemeService doctorDaySchemeService)
    {
        _doctorDaySchemeService = doctorDaySchemeService;
    }

    [HttpPut]
    [Route("update/{dayOfWeek}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> UpdateDoctorDaySchemeAsync(int dayOfWeek, [FromBody] DaySchemeDto daySchemeDto)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        await _doctorDaySchemeService.UpdateDoctorDaySchemeAsync(int.Parse(userId), dayOfWeek, daySchemeDto);
        return Ok();
    }
}