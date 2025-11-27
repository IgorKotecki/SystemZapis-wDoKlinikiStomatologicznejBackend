using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IDoctorService
{
    public Task AddDoctorAsync(AddDoctorDto addDoctorDto);
    Task DeleteDoctorAsync(int doctorId);
    Task<IEnumerable<DoctorDTO>> GetDoctorsAsync();
}