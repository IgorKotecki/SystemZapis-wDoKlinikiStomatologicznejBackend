using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    [Route("guest")]
    public async Task<IActionResult> PostGuestAppointmentAsync([FromBody] AppointmentRequest appointmentRequest)
    {
        try
        {
            await _appointmentService.CreateAppointmentGuestAsync(appointmentRequest);
            return Created();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("user/{userId}")]
    public async Task<IActionResult> GetAppointmentsByUserIdAsync(int userId, [FromQuery] string lang)
    {
        try
        {
            var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId, lang);
            if (!appointments.Any())
            {
                return NotFound("No appointments found for the specified user.");
            }

            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("user/book")]
    [Authorize(Roles = "Registered_user")]
    public async Task<IActionResult> BookAppointmentForRegisteredUserAsync(
        [FromBody] BookAppointmentRequestDTO bookAppointmentRequestDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var success =
                await _appointmentService.BookAppointmentForRegisteredUserAsync(int.Parse(userId),
                    bookAppointmentRequestDto);
            if (success)
            {
                return Ok("Appointment booked successfully.");
            }

            return BadRequest("Failed to book appointment.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error booking appointment: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("/api/DoctorAppointments")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorAppointments([FromQuery] string lang, [FromQuery] DateTime date)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var appointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(int.Parse(userId), lang, date);
            if (!appointments.Any())
            {
                return NotFound("No appointments found for the specified doctor.");
            }

            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [Route("createAddInformation")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> CreateAddInformationAsync([FromBody] AddInformationDto addInformationDto)
    {
        try
        {
            await _appointmentService.CreateAddInformationAsync(addInformationDto);
            return Ok("Additional information added successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error adding information: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("AddInfo")]
    [Authorize(Roles = "Doctor")]
    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync([FromQuery] string lang)
    {
        return await _appointmentService.GetAddInformationAsync(lang);
    }
}