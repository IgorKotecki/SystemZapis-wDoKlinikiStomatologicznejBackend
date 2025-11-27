using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _DoctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _DoctorRepository = doctorRepository;
    }

    public async Task AddDoctorAsync(AddDoctorDto addDoctorDto)
    {
        await _DoctorRepository.AddDoctorAsync(addDoctorDto);
    }

    public async Task DeleteDoctorAsync(int doctorId)
    {
        await _DoctorRepository.DeleteDoctorAsync(doctorId);
    }

    public async Task<IEnumerable<DoctorDTO>> GetDoctorsAsync()
    {
        var doctors = await _DoctorRepository.GetDoctorsAsync();

        return doctors.Select(d => new DoctorDTO
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