using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface IAdditionalInformationService
{
    Task<AddInformationOutDto> CreateAddInformationAsync(AddInformationDto addInformationDto);
    Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang);
    Task<AddInformationOutDto> GetAddInformationByIdAsync(int id, string lang);
    Task DeleteAddInformationByIdAsync(int id);
}