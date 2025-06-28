using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IServiceService
{
    public Task<ICollection<ServiceDTO>> GetAllServicesAvailableForClientWithLangAsync(string lang);
}