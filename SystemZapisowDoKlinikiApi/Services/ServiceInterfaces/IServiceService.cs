using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface IServiceService
{
    public Task<ICollection<ServiceDto>> GetAllServicesAvailableForClientWithLangAsync(string lang);
    public Task AddServiceAsync(AddServiceDto addServiceDto);
    public Task<AllServicesDto> GerAllServicesAsync(string lang);
    public Task<Service?> GetServiceByIdAsync(int serviceId);
    public Task<ServiceNameDTO> GetServiceNameDTOByIdAsync(int serviceId);
    Task<ServiceEditDto?> GetServiceForEditAsync(int serviceId);

    Task DeleteServiceAsync(int serviceId);
    public Task EditServiceAsync(int serviceId, ServiceEditDto editDto);
    public Task<List<ServiceCategoryDto>> GetAllServiceCategories();
    Task<ICollection<ServiceDto>> GetAllServicesForReceptionistAsync(string lang);
    Task<ICollection<ServiceDto>> GetAllServicesAsyncNoCategorySplits(string lang);
}