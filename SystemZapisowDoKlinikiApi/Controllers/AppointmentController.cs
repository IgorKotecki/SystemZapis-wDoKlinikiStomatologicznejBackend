using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.Attributes;
using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    [HttpPost("guest/appointment")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> BookGuestAppointmentAsync([FromBody] AppointmentRequest appointmentRequest)
    {
        await _appointmentService.CreateAppointmentGuestAsync(appointmentRequest);
        return Created();
    }

    [HttpGet("registered/appointments")]
    [Authorize(Roles = "Registered_user,Receptionist,Doctor,Admin")]
    public async Task<IActionResult> GetAppointmentsByUserIdAsync(
        [FromQuery] string lang,
        [FromQuery] bool showCancelled = true,
        [FromQuery] bool showCompleted = true,
        [FromQuery] bool showPlanned = true,
        [FromQuery] int userId = 0
    )
    {
        if (userId == 0)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Doctor" && userRole != "Receptionist")
            {
                userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            }

            if (userId == 0)
            {
                return Unauthorized();
            }
        }

        _logger.LogInformation("Getting appointments for user with id: {userId}, with language preference: {Lang}",
            userId, lang);

        var appointments = await _appointmentService
            .GetAppointmentsByUserIdAsync(
                userId,
                lang,
                showCancelled,
                showCompleted,
                showPlanned);

        return Ok(appointments);
    }

    [HttpPost("registered/appointment")]
    [Authorize(Roles = "Registered_user")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> BookAppointmentForRegisteredUserAsync(
        [FromBody] BookAppointmentRequestDto bookAppointmentRequestDto)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        _logger.LogInformation("Booking appointment for registered user with id: {userId}", userId);

        await _appointmentService.BookAppointmentForRegisteredUserAsync(
            int.Parse(userId),
            bookAppointmentRequestDto);

        return Created();
    }

    [HttpPost("receptionist/appointment")]
    [Authorize(Roles = "Receptionist,Doctor")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> BookAppointmentForRegisteredUserByReceptionistAsync(
        [FromBody] BookAppointmentRequestWithUserIdDto bookAppointmentRequestByReceptionistDto)
    {
        _logger.LogInformation(
            "Booking appointment for registered user with id: {userId} by receptionist/admin",
            bookAppointmentRequestByReceptionistDto.UserId);

        await _appointmentService.BookAppointmentForRegisteredUserAsync(
            bookAppointmentRequestByReceptionistDto.UserId,
            bookAppointmentRequestByReceptionistDto.BookAppointmentRequestDto);

        return Created();
    }

    [HttpGet("doctor/appointments")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorAppointments(
        [FromQuery] string lang,
        [FromQuery] DateTime date,
        [FromQuery] bool showCancelled = false,
        [FromQuery] bool showCompleted = false
    )
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        _logger.LogInformation(
            "Getting appointments for doctor with id: {userId}, for date: {Date}, with language preference: {Lang}",
            userId, date, lang);

        var appointments = await _appointmentService
            .GetAppointmentsByDoctorIdAsync(int.Parse(userId), lang, date, showCancelled, showCompleted);

        return Ok(appointments);
    }

    [HttpGet("receptionist/appointments")]
    [Authorize(Roles = "Receptionist,Admin")]
    public async Task<IActionResult> GetAppointmentsForReceptionist(
        [FromQuery] string lang,
        [FromQuery] DateTime date,
        [FromQuery] bool showCancelled = false,
        [FromQuery] bool showCompleted = false
    )
    {
        _logger.LogInformation(
            "Getting appointments for receptionist/admin for date: {Date}, with language preference: {Lang}",
            date, lang);

        var appointments = await _appointmentService
            .GetAppointmentsForReceptionistAsync(lang, date, showCancelled, showCompleted);

        return Ok(appointments);
    }

    [HttpPut("doctor/additional-information")]
    [Authorize(Roles = "Doctor")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> AddInfoToAppointmentAsync(
        [FromBody] AddInfoToAppointmentDto addInfoToAppointmentDto)
    {
        _logger.LogInformation("Adding additional information to appointment with id: {AppointmentId}",
            addInfoToAppointmentDto.Id);
        await _appointmentService.AddInfoToAppointmentAsync(addInfoToAppointmentDto);
        return Ok("Information added to appointment successfully.");
    }

    [HttpPut("appointment-status")]
    [Authorize(Roles = "Doctor,Admin,Receptionist")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> UpdateAppointmentStatusAsync(
        [FromBody] UpdateAppointmentStatusDto updateAppointmentStatusDto)
    {
        _logger.LogInformation("Updating status : {statusId} for appointment with id: {AppointmentId}",
            updateAppointmentStatusDto.StatusId, updateAppointmentStatusDto.AppointmentId);
        await _appointmentService.UpdateAppointmentStatusAsync(updateAppointmentStatusDto);
        return Ok("Appointment status updated successfully.");
    }

    [HttpPut("cancel-appointment")]
    [Authorize(Roles = "Registered_user,Doctor,Admin,Receptionist")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> CancelAppointmentAsync(
        [FromBody] CancellationDto cancellationDto)
    {
        _logger.LogInformation("Cancelling appointment with id: {AppointmentId}, reason: {Reason}",
            cancellationDto.AppointmentGuid, cancellationDto.Reason);
        await _appointmentService.CancelAppointmentAsync(cancellationDto);
        return Ok("Appointment cancelled successfully.");
    }

    [HttpPut("complete-appointment")]
    [Authorize(Roles = "Doctor,Admin,Receptionist")]
    [ConcurrentRequestLimit]
    public async Task<IActionResult> CompleteAppointmentAsync(
        [FromBody] CompletionDto completionDto)
    {
        _logger.LogInformation("Completing appointment with id: {AppointmentId}", completionDto.AppointmentGroupId);
        await _appointmentService.CompleteAppointmentAsync(completionDto);
        return Ok("Appointment completed successfully.");
    }
}