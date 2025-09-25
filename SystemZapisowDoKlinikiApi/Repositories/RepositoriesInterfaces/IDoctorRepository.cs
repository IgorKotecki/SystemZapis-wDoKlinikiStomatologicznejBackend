using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IDoctorRepository
{
    Task AddDoctorAsync(AddDoctorDto addDoctorDto);
    Task DeleteDoctorAsync(int doctorId);
}