using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface IAdditionalInformationRepository
{
    Task<AddInformationOutDto> CreateAddInformationAsync(AddInformationDto addInformationDto);
    Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang);
    Task<AddInformationOutDto> GetAddInformationByIdAsync(int id, string lang);
    Task DeleteAddInformationByIdAsync(int id);
}