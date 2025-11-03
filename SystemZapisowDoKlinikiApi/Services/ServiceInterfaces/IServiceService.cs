using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IServiceService
{
    public Task<ICollection<ServiceDTO>> GetAllServicesAvailableForClientWithLangAsync(string lang);
    public Task AddServiceAsync(AddServiceDto addServiceDto);
    public Task <ICollection<ServiceDTO>> GerAllServicesAsync(string lang);
}