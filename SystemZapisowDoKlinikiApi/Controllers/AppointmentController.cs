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
    public async Task<IActionResult> PostAppointment([FromBody] AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest == null)
        {
            return BadRequest("Invalid appointment request.");
        }

        try
        {
            await _appointmentService.CreateAppointmentAsync(appointmentRequest);
            return CreatedAtAction(nameof(PostAppointment), appointmentRequest);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}