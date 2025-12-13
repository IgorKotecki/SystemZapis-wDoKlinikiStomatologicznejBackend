using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IAppointmentRepository
{
    public Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest, int userId);
    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang);
    Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, DateTime date, string lang);
    Task<bool> BookAppointmentForRegisteredUserAsync(int userId, BookAppointmentRequestDTO bookAppointmentRequestDto);
}