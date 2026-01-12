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
        CheckAddInformationDto(addInformationDto);
        var newAddIfno = await _additionalInformationRepository.CreateAddInformationAsync(addInformationDto);
        return newAddIfno;
    }

    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang)
    {
        return await _additionalInformationRepository.GetAddInformationAsync(lang);
    }

    private void CheckAddInformationDto(AddInformationDto addInformationDto)
    {
        if (addInformationDto == null)
        {
            throw new ArgumentNullException(nameof(addInformationDto), "AddInformationDto cannot be null.");
        }

        if (addInformationDto.BodyPl == null || addInformationDto.BodyPl.Trim() == "")
        {
            throw new ArgumentException("BodyPlan cannot be null or empty.", nameof(addInformationDto.BodyPl));
        }

        if (addInformationDto.BodyEn == null || addInformationDto.BodyEn.Trim() == "")
        {
            throw new ArgumentException("BodyEn cannot be null or empty.", nameof(addInformationDto.BodyEn));
        }
    }
}