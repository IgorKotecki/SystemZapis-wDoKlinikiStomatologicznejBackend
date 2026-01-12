using SystemZapisowDoKlinikiApi.DTO.ToothDtos;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Services;

public class ToothService : IToothService
{
    private readonly IToothRepository _toothRepository;

    public ToothService(IToothRepository toothRepository)
    {
        _toothRepository = toothRepository;
    }

    public async Task<ToothModelOutDto> GetToothModelAsync(ToothModelRequest request)
    {
        return await _toothRepository.GetToothModelAsync(request);
    }

    public async Task UpdateToothModelAsync(ToothModelInDto request)
    {
        await _toothRepository.UpdateToothModelAsync(request);
    }

    public async Task<ToothStatusesDto> GetToothStatusesAsync(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            throw new ArgumentException("Language parameter is required", nameof(language));
        }

        if (language != "pl" && language != "en")
        {
            throw new ArgumentException("Unsupported language code. Supported codes are 'pl' and 'en'.",
                nameof(language));
        }

        return await _toothRepository.GetToothStatusesAsync(language);
    }
}