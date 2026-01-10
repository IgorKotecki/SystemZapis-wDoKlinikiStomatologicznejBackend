using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Controllers;

public interface IAdditionalInformationService
{
    Task<AddInformationOutDto> CreateAddInformationAsync(AddInformationDto addInformationDto);
    Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang);
    Task<AddInformationOutDto> GetAddInformationByIdAsync(int id, string lang);
    Task DeleteAddInformationByIdAsync(int id);
}