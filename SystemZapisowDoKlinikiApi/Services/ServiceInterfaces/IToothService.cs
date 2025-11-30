using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Controllers;

public interface IToothService
{
    public Task<ToothModelOutDTO> GetToothModelAsync(ToothModelRequest request);
    Task UpdateToothModelAsync(ToothModelInDTO request);
    Task<ToothStatusesDto> GetToothStatusesAsync(string language);
}