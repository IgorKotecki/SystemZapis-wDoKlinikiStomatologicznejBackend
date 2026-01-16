using System.Globalization;
using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;
using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Exceptions;
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
        ValidateAppointmentRequest(appointmentRequest);

        var user = await GetOrCreateGuestUserAsync(appointmentRequest);

        var bookAppointmentRequestDto = MapToBookAppointmentRequest(appointmentRequest);

        await _appointmentRepository.CreateAppointmentGuestAsync(bookAppointmentRequestDto, user.Id);

        await SendConfirmationEmailAsync(user, appointmentRequest.StartTime);
    }


    public async Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(
        int userId,
        string lang,
        bool showCancelled,
        bool showCompleted,
        bool showPlanned
    )
    {
        ValidateUserId(userId);

        return await FetchAppointmentsByUserAsync(userId, lang, showCancelled, showCompleted, showPlanned);
    }

    public static void ValidateUserId(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be a positive integer.", nameof(userId));
        }
    }

    private async Task<ICollection<AppointmentDto>> FetchAppointmentsByUserAsync(
        int userId,
        string lang,
        bool showCancelled,
        bool showCompleted,
        bool showPlanned
    )
    {
        return await _appointmentRepository.GetAppointmentsByUserIdAsync(userId, lang, showCancelled, showCompleted,
            showPlanned);
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(
        int doctorId,
        string lang,
        DateTime date,
        bool showCancelled,
        bool showCompleted)
    {
        ValidateDoctorId(doctorId);

        var mondayDate = GetMonday(date);

        return await FetchAppointmentsByDoctorAsync(doctorId, lang, mondayDate, showCancelled, showCompleted);
    }

    private static void ValidateDoctorId(int doctorId)
    {
        if (doctorId <= 0)
        {
            throw new ArgumentException("Doctor ID must be a positive integer.", nameof(doctorId));
        }
    }

    private async Task<ICollection<AppointmentDto>> FetchAppointmentsByDoctorAsync(
        int doctorId,
        string lang,
        DateTime mondayDate,
        bool showCancelled,
        bool showCompleted)
    {
        return await _appointmentRepository
            .GetAppointmentsByDoctorIdAsync(doctorId, mondayDate, lang, showCancelled, showCompleted);
    }

    public async Task BookAppointmentForRegisteredUserAsync(int userId,
        BookAppointmentRequestDto bookAppointmentRequestDto)
    {
        ValidateBookAppointmentRequest(bookAppointmentRequestDto);

        var user = await GetRegisteredUserAsync(userId);

        await _appointmentRepository.BookAppointmentForRegisteredUserAsync(userId, bookAppointmentRequestDto);

        await SendAppointmentConfirmationEmailAsync(user, bookAppointmentRequestDto.StartTime);
    }

    private static void ValidateBookAppointmentRequest(BookAppointmentRequestDto bookAppointmentRequestDto)
    {
        if (bookAppointmentRequestDto.StartTime < DateTime.Now)
        {
            throw new BusinessException("APPOINTMENT_IN_PAST", "Cannot book an appointment in the past.");
        }
    }

    private async Task<UserDto> GetRegisteredUserAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new InvalidOperationException("User should not be null at this point.");
        }

        return user;
    }

    private async Task SendAppointmentConfirmationEmailAsync(UserDto user, DateTime startTime)
    {
        await _emailService.SendEmailAsync(
            user.Email,
            "Appointment Confirmation",
            $"Dear {user.Name},\n\nYour appointment has been successfully booked.\n Date : {startTime}.\n\nBest regards,\nClinic Team"
        );
    }

    public async Task AddInfoToAppointmentAsync(AddInfoToAppointmentDto addInfoToAppointmentDto)
    {
        await _appointmentRepository.AddInfoToAppointmentAsync(addInfoToAppointmentDto);
    }

    public async Task UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto)
    {
        ValidateUpdateAppointmentStatusDto(updateAppointmentStatusDto);

        await UpdateStatusInRepositoryAsync(updateAppointmentStatusDto);
    }

    private static void ValidateUpdateAppointmentStatusDto(UpdateAppointmentStatusDto updateAppointmentStatusDto)
    {
        if (updateAppointmentStatusDto == null)
        {
            throw new ArgumentNullException(nameof(updateAppointmentStatusDto),
                "UpdateAppointmentStatusDto cannot be null.");
        }

        if (updateAppointmentStatusDto.StatusId <= 0 || updateAppointmentStatusDto.StatusId > 4)
        {
            throw new ArgumentException("StatusId don't match any status", nameof(updateAppointmentStatusDto.StatusId));
        }
    }

    private async Task UpdateStatusInRepositoryAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto)
    {
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
        var appointment = await GetAppointmentByGuidAsync(cancellationDto.AppointmentGuid);

        ValidateCancellation(appointment);

        await _appointmentRepository.CancelAppointmentAsync(cancellationDto);

        NotifyUserAboutCancellation(appointment);
    }

    private async Task<AppointmentDto> GetAppointmentByGuidAsync(string appointmentGuid)
    {
        return await _appointmentRepository.GetAppointmentsByGuidAsync(appointmentGuid);
    }

    private static void ValidateCancellation(AppointmentDto appointment)
    {
        if (appointment.StartTime < DateTime.Now)
        {
            throw new BusinessException(
                "CANCELLATION_OF_PAST_APPOINTMENT",
                "Cannot cancel an appointment that is in the past."
            );
        }
    }

    private void NotifyUserAboutCancellation(AppointmentDto appointment)
    {
        var user = appointment.User;
        var email = user.Email;

        //TODO Uncomment and implement email sending logic when ready
        // await _emailService.SendEmailAsync(email, "Appointment Cancellation",
        //     $"Dear {user.Name},\n\nYour appointment scheduled on {appointment.StartTime} has been cancelled.\n\nBest regards,\nClinic Team");
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

    private static void ValidateAppointmentRequest(AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest == null)
            throw new ArgumentNullException(nameof(appointmentRequest), "Appointment request cannot be null.");

        EnsureAppointmentIsNotInPast(appointmentRequest.StartTime);
    }

    public static void EnsureAppointmentIsNotInPast(DateTime startTime)
    {
        if (startTime < DateTime.Now)
            throw new BusinessException(
                "APPOINTMENT_IN_PAST",
                "Cannot book an appointment in the past."
            );
    }

    private async Task<User> GetOrCreateGuestUserAsync(AppointmentRequest appointmentRequest)
    {
        var user = await _userService.GetUserByEmailAsync(appointmentRequest.Email);

        if (user == null)
        {
            return await _userService.CreateGuestUserAsync(
                appointmentRequest.Name,
                appointmentRequest.Surname,
                appointmentRequest.Email,
                appointmentRequest.PhoneNumber
            );
        }

        CheckUserDataForAppointment(appointmentRequest, user);
        return user;
    }

    private static BookAppointmentRequestDto MapToBookAppointmentRequest(AppointmentRequest appointmentRequest)
    {
        return new BookAppointmentRequestDto
        {
            DoctorId = appointmentRequest.DoctorId,
            StartTime = appointmentRequest.StartTime,
            Duration = appointmentRequest.Duration,
            ServicesIds = appointmentRequest.ServicesIds
        };
    }

    public async Task SendConfirmationEmailAsync(User user, DateTime startTime)
    {
        await _emailService.SendEmailAsync(
            user.Email,
            "Appointment Confirmation",
            $"Dear {user.Name},\n\nYour appointment has been successfully booked.\n Date : {startTime.ToString(CultureInfo.InvariantCulture)}.\n\nBest regards,\nClinic Team"
        );
    }
}