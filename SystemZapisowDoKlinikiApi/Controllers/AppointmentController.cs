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
            return CreatedAtAction(string.Empty, appointmentRequest);
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
    public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentRequestDTO bookAppointmentRequestDto)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var success = await _appointmentService.BookAppointmentAsync(int.Parse(userId), bookAppointmentRequestDto);
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
}