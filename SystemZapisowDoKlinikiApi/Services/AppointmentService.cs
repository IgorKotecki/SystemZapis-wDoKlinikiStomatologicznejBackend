using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserService _userService;
    private readonly ITimeBlockService _timeBlockService;

    public AppointmentService(IAppointmentRepository appointmentRepository, IUserService userService,
        ITimeBlockService timeBlockService)
    {
        _appointmentRepository = appointmentRepository;
        _userService = userService;
        _timeBlockService = timeBlockService;
    }

    public async Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest == null)
        {
            throw new ArgumentNullException(nameof(appointmentRequest), "Appointment request cannot be null.");
        }

        foreach (var id in appointmentRequest.DoctorBlockId)
        {
            var timeBlock = await _timeBlockService.GetTimeBlockByDoctorBlockIdAsync(id);
            if (timeBlock == null)
            {
                throw new ArgumentException($"Time block with ID {id} does not exist.",
                    nameof(appointmentRequest.DoctorBlockId));
            }

            if (timeBlock.TimeStart < DateTime.Now)
            {
                throw new ArgumentException($"Time block with ID {id} is in the past.",
                    nameof(appointmentRequest.DoctorBlockId));
            }
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
            CheckUserDataForAppointment(appointmentRequest, user);
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

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, string lang,
        DateTime date)
    {
        if (doctorId <= 0)
        {
            throw new ArgumentException("Doctor ID must be a positive integer.", nameof(doctorId));
        }

        var mondayDate = GetMonday(date);
        return await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId, mondayDate, lang);
    }

    public async Task<bool> BookAppointmentForRegisteredUserAsync(int userId,
        BookAppointmentRequestDTO bookAppointmentRequestDto)
    {
        return await _appointmentRepository.BookAppointmentForRegisteredUserAsync(userId, bookAppointmentRequestDto);
    }

    private void CheckUserDataForAppointment(AppointmentRequest appointmentRequest, User user)
    {
        if (user.Name != appointmentRequest.Name ||
            user.Surname != appointmentRequest.Surname ||
            user.Email != appointmentRequest.Email ||
            user.PhoneNumber != appointmentRequest.PhoneNumber)
        {
            throw new ArgumentException("Data in appointment don't mach user with specified email.",
                nameof(appointmentRequest));
        }
    }

    private static DateTime GetMonday(DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }
}