using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Services;

public interface IServiceService
{
    public Task<ICollection<ServiceDTO>> GetAllServicesAvailableForClientWithLangAsync(string lang);
    public Task AddServiceAsync(AddServiceDto addServiceDto);
    public Task<AllServicesDto> GerAllServicesAsync(string lang);
    public Task<Service?> GetServiceByIdAsync(int serviceId);
    Task<ServiceEditDTO?> GetServiceForEditAsync(int serviceId);

    Task DeleteServiceAsync(int serviceId);
    public Task EditServiceAsync(int serviceId, ServiceEditDTO editDto);
    public Task<List<ServiceCategoryDTO>> GetAllServiceCategories();
    Task<ICollection<ServiceDTO>> GetAllServicesForReceptionistAsync(string lang);
}