using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IAppointmentService
{
    public Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest);
    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang);
    public Task<bool> BookAppointmentAsync(int userId, BookAppointmentRequestDTO bookAppointmentRequestDto);
}