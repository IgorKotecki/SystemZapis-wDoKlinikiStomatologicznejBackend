using System.Security.Claims;
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
    public async Task<IActionResult> PostGuestAppointment([FromBody] AppointmentRequest appointmentRequest)
    {
        try
        {
            await _appointmentService.CreateAppointmentGuestAsync(appointmentRequest);
            return CreatedAtAction(nameof(PostGuestAppointment), appointmentRequest);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("user/{userId}")]
    public async Task<IActionResult> GetAppointmentsByUserId(int userId, [FromQuery] string lang)
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
    public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentRequestDTO bookAppointmentRequestDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User is not authenticated.");
            }
            var userId = int.Parse(userIdClaim);
            var success = await _appointmentService.BookAppointmentAsync(userId, bookAppointmentRequestDto);
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