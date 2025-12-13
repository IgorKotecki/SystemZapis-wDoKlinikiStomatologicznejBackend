using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface IServiceRepository
{
    public Task<ICollection<ServiceDTO>> GetAllServicesAvailableForClientWithLangAsync(string lang);
    public Task<Service?> GetServiceByIdAsync(int serviceId);
    public Task AddServiceAsync(AddServiceDto addServiceDto);
    public Task<AllServicesDto> GetAllServicesAsync(string lang);
    Task DeleteServiceAsync(int serviceId);
}