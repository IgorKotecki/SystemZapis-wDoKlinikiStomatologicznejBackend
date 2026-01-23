using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface IServiceRepository
{
    public Task<ICollection<ServiceDto>> GetAllServicesAvailableForClientWithLangAsync(string lang);
    public Task<Service?> GetServiceByIdAsync(int serviceId);
    public Task<ServiceNameDTO> GetServiceNameDTOByIdAsync(int serviceId);
    public Task<ServiceEditDto?> GetServiceEditDtoByIdAsync(int serviceId);
    public Task AddServiceAsync(AddServiceDto addServiceDto);
    public Task<AllServicesDto> GetAllServicesAsync(string lang);
    Task DeleteServiceAsync(int serviceId);
    Task EditServiceAsync(int serviceId, ServiceEditDto serviceEditDTO);
    public Task<List<ServiceCategory>> GetAllServiceCategories();
    Task<ICollection<ServiceDto>> GetAllServicesForReceptionistAsync(string lang);
    Task<ICollection<ServiceDto>> GetAllServicesAsyncNoCategorySplits(string lang);
}