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
}