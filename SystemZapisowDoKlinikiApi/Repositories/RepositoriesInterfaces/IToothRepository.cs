using SystemZapisowDoKlinikiApi.DTO.ToothDtos;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface IToothRepository
{
    public Task<ToothModelOutDto> GetToothModelAsync(ToothModelRequest request);
    Task UpdateToothModelAsync(ToothModelInDto request);
    Task<ToothStatusesDto> GetToothStatusesAsync(string language);
    Task CreateTeethModelForUserAsync(CreateToothModelDto request);
}