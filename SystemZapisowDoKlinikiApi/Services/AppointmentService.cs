using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.Controllers;
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

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be a positive integer.", nameof(userId));
        }

        var appointments = await _appointmentRepository.GetAppointmentsByUserIdAsync(userId, lang);

        if (appointments.IsNullOrEmpty())
        {
            throw new KeyNotFoundException($"No appointments found for user with ID {userId}.");
        }

        return appointments;
    }

    public async Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, string lang,
        DateTime date)
    {
        if (doctorId <= 0)
        {
            throw new ArgumentException("Doctor ID must be a positive integer.", nameof(doctorId));
        }

        var mondayDate = GetMonday(date);

        var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId, mondayDate, lang);

        if (appointments.IsNullOrEmpty())
        {
            throw new KeyNotFoundException($"No appointments found for doctor with ID {doctorId}.");
        }

        return appointments;
    }

    public async Task BookAppointmentForRegisteredUserAsync(int userId,
        BookAppointmentRequestDTO bookAppointmentRequestDto)
    {
        await _appointmentRepository.BookAppointmentForRegisteredUserAsync(userId, bookAppointmentRequestDto);
    }

    public async Task CreateAddInformationAsync(AddInformationDto addInformationDto)
    {
        CheckAddInformationDto(addInformationDto);
        await _appointmentRepository.CreateAddInformationAsync(addInformationDto);
    }

    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang)
    {
        return await _appointmentRepository.GetAddInformationAsync(lang);
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

    private void CheckAddInformationDto(AddInformationDto addInformationDto)
    {
        if (addInformationDto == null)
        {
            throw new ArgumentNullException(nameof(addInformationDto), "AddInformationDto cannot be null.");
        }

        if (addInformationDto.BodyPl == null || addInformationDto.BodyPl.Trim() == "")
        {
            throw new ArgumentException("BodyPlan cannot be null or empty.", nameof(addInformationDto.BodyPl));
        }

        if (addInformationDto.BodyEn == null || addInformationDto.BodyEn.Trim() == "")
        {
            throw new ArgumentException("BodyEn cannot be null or empty.", nameof(addInformationDto.BodyEn));
        }
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