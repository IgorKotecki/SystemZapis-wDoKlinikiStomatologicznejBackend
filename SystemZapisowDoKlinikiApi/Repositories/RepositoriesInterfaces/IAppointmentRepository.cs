using SystemZapisowDoKlinikiApi.Controllers;
using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IAppointmentRepository
{
    public Task CreateAppointmentGuestAsync(BookAppointmentRequestDTO appointmentRequest, int userId);
    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang);

    Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(
        int doctorId,
        DateTime date,
        string lang,
        bool showCancelled,
        bool showCompleted);

    Task BookAppointmentForRegisteredUserAsync(int userId, BookAppointmentRequestDTO bookAppointmentRequestDto);
    Task AddInfoToAppointmentAsync(AddInfoToAppointmentDto addInfoToAppointmentDto);
    Task UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto);

    Task<ICollection<AppointmentDto>> GetAppointmentsForReceptionistAsync(
        DateTime mondayDate,
        string lang,
        bool showCancelled,
        bool showCompleted
    );

    Task<ICollection<AppointmentDto>> GetAppointmentsByDateAsync(string lang, DateTime date);
    Task CancelAppointmentAsync(CancellationDto appointmentGuid);
    Task<AppointmentDto> GetCancelledAppointmentByIdAsync(string appointmentGuid);
    Task CompleteAppointmentAsync(CompletionDto completionDto);
}