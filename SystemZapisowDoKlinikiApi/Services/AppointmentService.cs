using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Security;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IUserService userService,
        IEmailService emailService
    )
    {
        _appointmentRepository = appointmentRepository;
        _userService = userService;

        _emailService = emailService;
    }

    public async Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest == null)
        {
            throw new ArgumentNullException(nameof(appointmentRequest), "Appointment request cannot be null.");
        }

        // if (appointmentRequest.StartTime < DateTime.Now)
        // {
        //     throw new BusinessException("APPOINTMENT_IN_PAST",
        //         "Cannot book an appointment in the past.");
        // }

        var user = await _userService.GetUserByEmailAsync(appointmentRequest.Email);

        if (user == null)
        {
            user = await _userService.CreateGuestUserAsync(
                appointmentRequest.Name,
                appointmentRequest.Surname,
                appointmentRequest.Email,
                appointmentRequest.PhoneNumber
            );
        }
        else
        {
            CheckUserDataForAppointment(appointmentRequest, user);
        }

        var bookAppointmentRequestDto = new BookAppointmentRequestDto
        {
            DoctorId = appointmentRequest.DoctorId,
            StartTime = appointmentRequest.StartTime,
            Duration = appointmentRequest.Duration,
            ServicesIds = appointmentRequest.ServicesIds
        };


        await _appointmentRepository.CreateAppointmentGuestAsync(bookAppointmentRequestDto, user.Id);

        await _emailService.SendEmailAsync(user.Email, "Appointment Confirmation",
            $"Dear {user.Name},\n\nYour appointment has been successfully booked.\n Date : {appointmentRequest.StartTime}.\n\nBest regards,\nClinic Team");
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be a positive integer.", nameof(userId));
        }

        var appointments = await _appointmentRepository.GetAppointmentsByUserIdAsync(userId, lang);

        return appointments;
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(
        int doctorId,
        string lang,
        DateTime date,
        bool showCancelled,
        bool showCompleted)
    {
        if (doctorId <= 0)
        {
            throw new ArgumentException("Doctor ID must be a positive integer.", nameof(doctorId));
        }

        var mondayDate = GetMonday(date);

        var appointments = await _appointmentRepository
            .GetAppointmentsByDoctorIdAsync(doctorId, mondayDate, lang, showCancelled, showCompleted);

        return appointments;
    }

    public async Task BookAppointmentForRegisteredUserAsync(int userId,
        BookAppointmentRequestDto bookAppointmentRequestDto)
    {
        await _appointmentRepository.BookAppointmentForRegisteredUserAsync(userId, bookAppointmentRequestDto);

        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException("User should not be null at this point.");
        }

        await _emailService.SendEmailAsync(user.Email, "Appointment Confirmation",
            $"Dear {user.Name},\n\nYour appointment has been successfully booked.\n Date : {bookAppointmentRequestDto.StartTime}.\n\nBest regards,\nClinic Team");
    }


    public async Task AddInfoToAppointmentAsync(AddInfoToAppointmentDto addInfoToAppointmentDto)
    {
        await _appointmentRepository.AddInfoToAppointmentAsync(addInfoToAppointmentDto);
    }

    public async Task UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto)
    {
        if (updateAppointmentStatusDto == null)
        {
            throw new ArgumentNullException(nameof(updateAppointmentStatusDto),
                "UpdateAppointmentStatusDto cannot be null.");
        }

        if (updateAppointmentStatusDto.StatusId <= 0 || updateAppointmentStatusDto.StatusId > 4)
        {
            throw new ArgumentException("StatusId don't mach any status", nameof(updateAppointmentStatusDto.StatusId));
        }

        await _appointmentRepository.UpdateAppointmentStatusAsync(updateAppointmentStatusDto);
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsForReceptionistAsync(
        string lang,
        DateTime date,
        bool showCancelled,
        bool showCompleted)
    {
        var mondayDate = GetMonday(date);

        var appointments = await _appointmentRepository
            .GetAppointmentsForReceptionistAsync(mondayDate, lang, showCancelled, showCompleted);

        return appointments;
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDate(string lang, DateTime date)
    {
        return await _appointmentRepository.GetAppointmentsByDateAsync(lang, date);
    }

    public async Task CancelAppointmentAsync(CancellationDto cancellationDto)
    {
        await _appointmentRepository.CancelAppointmentAsync(cancellationDto);
        var appointment =
            await _appointmentRepository.GetCancelledAppointmentByIdAsync(cancellationDto.AppointmentGuid);
        var user = appointment.User;
        var email = user.Email;
        //await _emailService.SendEmailAsync(email, "Appointment Cancellation",
        //    $"Dear {user.Name},\n\nYour appointment scheduled on {appointment.StartTime} has been cancelled.\n\nBest regards,\nClinic Team");
    }

    public async Task CompleteAppointmentAsync(CompletionDto completionDto)
    {
        await _appointmentRepository.CompleteAppointmentAsync(completionDto);
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
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }
}