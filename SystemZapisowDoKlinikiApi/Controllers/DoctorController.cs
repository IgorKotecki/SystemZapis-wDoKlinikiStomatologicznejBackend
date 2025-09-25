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

    public DoctorController(IDoctorDaySchemeService doctorDaySchemeService, IDoctorService doctorService)
    {
        _doctorDaySchemeService = doctorDaySchemeService;
        _doctorService = doctorService;
    }

    [HttpPut]
    [Route("daySchemeUpdate/{dayOfWeek}")]
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

    [HttpPost]
    [Route("addDoctor")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDoctorAsync([FromBody] AddDoctorDto addDoctorDto)
    {
        try
        {
            await _doctorService.AddDoctorAsync(addDoctorDto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(e.Message);
        }
    }
    [HttpDelete]
    [Route("deleteDoctor/{doctorId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDoctorAsync(int doctorId)
    {
        try
        {
            await _doctorService.DeleteDoctorAsync(doctorId);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(e.Message);
        }
    }
}
