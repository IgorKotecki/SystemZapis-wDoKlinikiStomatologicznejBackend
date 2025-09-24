using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IAppointmentRepository
{
    public Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest, int userId);
    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang);
}