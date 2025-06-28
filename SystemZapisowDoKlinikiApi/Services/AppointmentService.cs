using System.Security.Claims;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentService(IAppointmentRepository appointmentRepository, IHttpContextAccessor httpContextAccessor)
    {
        _appointmentRepository = appointmentRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateAppointmentAsync(AppointmentRequest appointmentRequest)
    {
        var role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        Console.WriteLine($"Role: {role}");
        await _appointmentRepository.CreateAppointmentAsync(appointmentRequest, role);
    }
}