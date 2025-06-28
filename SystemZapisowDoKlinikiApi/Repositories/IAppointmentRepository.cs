using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IAppointmentRepository
{
    public Task CreateAppointmentAsync(AppointmentRequest appointmentRequest,string role);
}