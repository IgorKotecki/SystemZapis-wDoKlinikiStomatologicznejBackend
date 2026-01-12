using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task AddDoctorAsync(AddDoctorDto addDoctorDto)
    {
        await _doctorRepository.AddDoctorAsync(addDoctorDto);
    }

    public async Task DeleteDoctorAsync(int doctorId)
    {
        await _doctorRepository.DeleteDoctorAsync(doctorId);
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetDoctorsAsync();

        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name,
            Surname = d.Surname,
            Email = d.Email,
            SpecializationPl = d.Doctor?.SpecializationPl,
            SpecializationEn = d.Doctor?.SpecializationEn
        });
    }
}