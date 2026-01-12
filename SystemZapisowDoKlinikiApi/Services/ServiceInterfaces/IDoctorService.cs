using SystemZapisowDoKlinikiApi.DTO.UserDtos;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface IDoctorService
{
    public Task AddDoctorAsync(AddDoctorDto addDoctorDto);
    Task DeleteDoctorAsync(int doctorId);
    Task<IEnumerable<DoctorDto>> GetDoctorsAsync();
}