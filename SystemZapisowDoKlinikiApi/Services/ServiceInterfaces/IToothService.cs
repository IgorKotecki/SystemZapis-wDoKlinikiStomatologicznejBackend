using SystemZapisowDoKlinikiApi.DTO.ToothDtos;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface IToothService
{
    public Task<ToothModelOutDto> GetToothModelAsync(ToothModelRequest request);
    Task UpdateToothModelAsync(ToothModelInDto request);
    Task<ToothStatusesDto> GetToothStatusesAsync(string language);
    Task CreateTeethModelForUserAsync(CreateToothModelDto request);
}