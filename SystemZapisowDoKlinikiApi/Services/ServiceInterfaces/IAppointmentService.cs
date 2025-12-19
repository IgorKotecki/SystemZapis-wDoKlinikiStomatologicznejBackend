using SystemZapisowDoKlinikiApi.Controllers;
using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IAppointmentService
{
    public Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest);
    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang);
    Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, string lang, DateTime date);
    Task BookAppointmentForRegisteredUserAsync(int userId, BookAppointmentRequestDTO bookAppointmentRequestDto);
    Task CreateAddInformationAsync(AddInformationDto addInformationDto);
    Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang);
    Task AddInfoToAppointmentAsync(AddInfoToAppointmentDto addInfoToAppointmentDto);
    Task UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto);
    Task<ICollection<AppointmentDto>> GetAppointmentsForReceptionistAsync(string lang, DateTime date);
}