using System.Security.Claims;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserService _userService;

    public AppointmentService(IAppointmentRepository appointmentRepository, IUserService userService)
    {
        _appointmentRepository = appointmentRepository;
        _userService = userService;
    }

    public async Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest == null)
        {
            throw new ArgumentNullException(nameof(appointmentRequest), "Appointment request cannot be null.");
        }

        var user = await _userService.GetUserByEmailAsync(appointmentRequest.Email);
        var userId = 0;
        if (user == null)
        {
            userId = await _userService.CreateGuestUserAsync(
                appointmentRequest.Name,
                appointmentRequest.Surname,
                appointmentRequest.Email,
                appointmentRequest.PhoneNumber
            );
        }
        else
        {
            userId = user.Id;
        }

        await _appointmentRepository.CreateAppointmentGuestAsync(appointmentRequest, userId);
    }

    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be a positive integer.", nameof(userId));
        }

        return _appointmentRepository.GetAppointmentsByUserIdAsync(userId, lang);
    }
}