using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface IAppointmentService
{
    public Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest);

    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(
        int userId,
        string lang,
        bool showCancelled,
        bool showCompleted,
        bool showPlanned
    );

    Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(
        int doctorId, string lang, DateTime date, bool showCancelled, bool showCompleted);

    Task BookAppointmentForRegisteredUserAsync(int userId, BookAppointmentRequestDto bookAppointmentRequestDto);
    Task AddInfoToAppointmentAsync(AddInfoToAppointmentDto addInfoToAppointmentDto);
    Task UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto);

    Task<ICollection<AppointmentDto>> GetAppointmentsForReceptionistAsync(
        string lang,
        DateTime date,
        bool showCancelled,
        bool showCompleted);

    Task<ICollection<AppointmentDto>> GetAppointmentsByDate(string lang, DateTime date);
    Task CancelAppointmentAsync(CancellationDto appointmentGuid);
    Task CompleteAppointmentAsync(CompletionDto completionDto);
}