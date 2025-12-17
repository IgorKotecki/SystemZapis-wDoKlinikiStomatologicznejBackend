using SystemZapisowDoKlinikiApi.Controllers;
using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IAppointmentRepository
{
    public Task CreateAppointmentGuestAsync(AppointmentRequest appointmentRequest, int userId);
    public Task<ICollection<AppointmentDto>> GetAppointmentsByUserIdAsync(int userId, string lang);
    Task<ICollection<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, DateTime date, string lang);
    Task BookAppointmentForRegisteredUserAsync(int userId, BookAppointmentRequestDTO bookAppointmentRequestDto);
    Task CreateAddInformationAsync(AddInformationDto addInformationDto);
    Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang);
    Task AddInfoToAppointmentAsync(AddInfoToAppointmentDto addInfoToAppointmentDto);
    Task UpdateAppointmentStatusAsync(UpdateAppointmentStatusDto updateAppointmentStatusDto);
}