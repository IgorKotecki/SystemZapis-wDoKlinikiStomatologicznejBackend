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
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    [HttpPost("guest/appointment")]
    public async Task<IActionResult> BookGuestAppointmentAsync([FromBody] AppointmentRequest appointmentRequest)
    {
        await _appointmentService.CreateAppointmentGuestAsync(appointmentRequest);
        return Created();
    }

    [HttpGet("registered/appointments")]
    [Authorize(Roles = "Registered_user,Doctor,Admin")]
    public async Task<IActionResult> GetAppointmentsByUserIdAsync([FromQuery] string lang)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        _logger.LogInformation("Getting appointments for user with id: {userId}, with language preference: {Lang}",
            userId, lang);

        var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(int.Parse(userId), lang);

        return Ok(appointments);
    }

    [HttpPost("registered/appointment")]
    [Authorize(Roles = "Registered_user")]
    public async Task<IActionResult> BookAppointmentForRegisteredUserAsync(
        [FromBody] BookAppointmentRequestDTO bookAppointmentRequestDto)
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
    [Authorize(Roles = "Receptionist,Admin")]
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
    public async Task<IActionResult> GetDoctorAppointments([FromQuery] string lang, [FromQuery] DateTime date)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }

        _logger.LogInformation(
            "Getting appointments for doctor with id: {userId}, for date: {Date}, with language preference: {Lang}",
            userId, date, lang);

        var appointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(int.Parse(userId), lang, date);

        return Ok(appointments);
    }

    [HttpGet("receptionist/appointments")]
    //[Authorize(Roles = "Receptionist,Admin")]
    public async Task<IActionResult> GetAppointmentsForReceptionist([FromQuery] string lang, [FromQuery] DateTime date)
    {
        _logger.LogInformation(
            "Getting appointments for receptionist/admin for date: {Date}, with language preference: {Lang}",
            date, lang);

        var appointments = await _appointmentService.GetAppointmentsForReceptionistAsync(lang, date);

        return Ok(appointments);
    }

    [HttpPost("doctor/additional-information")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> CreateAddInformationAsync([FromBody] AddInformationDto addInformationDto)
    {
        _logger.LogInformation("Creating additional information entry by doctor.");

        await _appointmentService.CreateAddInformationAsync(addInformationDto);
        return Ok("Additional information added successfully.");
    }

    [HttpGet("doctor/additional-information")]
    [Authorize(Roles = "Doctor")]
    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync([FromQuery] string lang)
    {
        _logger.LogInformation("Retrieving additional information entries for doctor with language preference: {Lang}",
            lang);
        return await _appointmentService.GetAddInformationAsync(lang);
    }

    [HttpPut("doctor/additional-information")]
    [Authorize(Roles = "Doctor")]
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
    public async Task<IActionResult> UpdateAppointmentStatusAsync(
        [FromBody] UpdateAppointmentStatusDto updateAppointmentStatusDto)
    {
        _logger.LogInformation("Updating status : {statusId} for appointment with id: {AppointmentId}",
            updateAppointmentStatusDto.StatusId, updateAppointmentStatusDto.AppointmentId);
        await _appointmentService.UpdateAppointmentStatusAsync(updateAppointmentStatusDto);
        return Ok("Appointment status updated successfully.");
    }
}