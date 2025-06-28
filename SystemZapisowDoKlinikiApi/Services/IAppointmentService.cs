using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IAppointmentService
{
    public Task CreateAppointmentAsync(AppointmentRequest appointmentRequest);
}