using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Services;

public class AdditionalInformationService : IAdditionalInformationService
{
    private readonly IAdditionalInformationRepository _additionalInformationRepository;

    public AdditionalInformationService(IAdditionalInformationRepository additionalInformationRepository)
    {
        _additionalInformationRepository = additionalInformationRepository;
    }

    public Task<AddInformationOutDto> GetAddInformationByIdAsync(int id, string lang)
    {
        return _additionalInformationRepository.GetAddInformationByIdAsync(id, lang);
    }

    public async Task DeleteAddInformationByIdAsync(int id)
    {
        await _additionalInformationRepository.DeleteAddInformationByIdAsync(id);
    }

    public async Task<AddInformationOutDto> CreateAddInformationAsync(AddInformationDto addInformationDto)
    {
        ValidateAddInformationDto(addInformationDto);

        return await _additionalInformationRepository.CreateAddInformationAsync(addInformationDto);
    }

    private static void ValidateAddInformationDto(AddInformationDto addInformationDto)
    {
        if (addInformationDto == null)
        {
            throw new ArgumentNullException(nameof(addInformationDto), "AddInformationDto cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(addInformationDto.BodyPl))
        {
            throw new ArgumentException("BodyPl cannot be null or empty.", nameof(addInformationDto.BodyPl));
        }

        if (string.IsNullOrWhiteSpace(addInformationDto.BodyEn))
        {
            throw new ArgumentException("BodyEn cannot be null or empty.", nameof(addInformationDto.BodyEn));
        }
    }

    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang)
    {
        return await _additionalInformationRepository.GetAddInformationAsync(lang);
    }
}