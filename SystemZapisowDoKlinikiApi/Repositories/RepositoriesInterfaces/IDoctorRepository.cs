using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface IDoctorRepository
{
    Task AddDoctorAsync(AddDoctorDto addDoctorDto);
    Task DeleteDoctorAsync(int doctorId);
    Task<User?> GetDoctorByIdAsync(int doctorId);
    Task<IEnumerable<User>> GetDoctorsAsync();
}