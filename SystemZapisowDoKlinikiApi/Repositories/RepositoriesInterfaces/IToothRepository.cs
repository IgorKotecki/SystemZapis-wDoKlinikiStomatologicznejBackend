using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IToothRepository
{
    public Task<ToothModelOutDTO> GetToothModelAsync(ToothModelRequest request);
    Task UpdateToothModelAsync(ToothModelInDTO request);
    Task<ToothStatusesDto> GetToothStatusesAsync(string language);
}