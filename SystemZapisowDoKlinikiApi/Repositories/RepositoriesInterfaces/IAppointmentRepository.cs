using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface IAppointmentRepository
{
    public Task CreateAppointmentGuestAsync(BookAppointmentRequestDto appointmentRequest, int userId);

    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(
        int userId,
        string lang,
        bool showCancelled,
        bool showCompleted,
        bool showPlanned
    );

    Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(
        int doctorId,
        DateTime date,
        string lang,
        bool showCancelled,
        bool showCompleted);

    Task BookAppointmentForRegisteredUserAsync(int userId, BookAppointmentRequestDto bookAppointmentRequestDto);
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