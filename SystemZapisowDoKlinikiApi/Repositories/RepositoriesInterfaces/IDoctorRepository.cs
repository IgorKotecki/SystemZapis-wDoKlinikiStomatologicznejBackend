using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IDoctorRepository
{
    Task AddDoctorAsync(AddDoctorDto addDoctorDto);
    Task DeleteDoctorAsync(int doctorId);
    Task<User?> GetDoctorByIdAsync(int doctorId);
    Task<IEnumerable<User>> GetDoctorsAsync();
}