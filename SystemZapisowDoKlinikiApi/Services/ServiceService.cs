using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ICollection<ServiceDTO>> GetAllServicesAvailableForClientWithLangAsync(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
        {
            throw new ArgumentException("Language code is required.");
        }

        var services = await _serviceRepository.GetAllServicesAvailableForClientWithLangAsync(lang);

        if (services == null || !services.Any())
        {
            throw new ArgumentException("No services found for the specified language.");
        }

        return services;
    }

    public async Task AddServiceAsync(AddServiceDto addServiceDto)
    {
        if (addServiceDto == null)
        {
            throw new ArgumentNullException(nameof(addServiceDto), "Service data is required.");
        }

        if (addServiceDto.LowPrice == null && addServiceDto.HighPrice == null)
        {
            throw new ArgumentException("At least one price (LowPrice or HighPrice) must be provided.");
        }

        await _serviceRepository.AddServiceAsync(addServiceDto);
    }

    public async Task<AllServicesDto> GerAllServicesAsync(string lang)
    {
        if (string.IsNullOrEmpty(lang))
            lang = "pl";

        var services = await _serviceRepository.GetAllServicesAsync(lang);

        if (services.ServicesByCategory.IsNullOrEmpty())
        {
            throw new KeyNotFoundException("No services found.");
        }

        return services;
    }

    public async Task<Service?> GetServiceByIdAsync(int serviceId)
    {
        return await _serviceRepository.GetServiceByIdAsync(serviceId);
    }

    public async Task<ServiceEditDTO?> GetServiceForEditAsync(int serviceId)
    {
        return await _serviceRepository.GetServiceEditDTOByIdAsync(serviceId);
    }


    public Task DeleteServiceAsync(int serviceId)
    {
        return _serviceRepository.DeleteServiceAsync(serviceId);
    }

    public Task EditServiceAsync(int serviceId, ServiceEditDTO serviceEditDto)
    {
        if (serviceEditDto == null)
            throw new ArgumentNullException(nameof(serviceEditDto));

        return _serviceRepository.EditServiceAsync(serviceId, serviceEditDto);
    }


    public async Task<List<ServiceCategoryDTO>> GetAllServiceCategories()
    {
        var categories = await _serviceRepository.GetAllServiceCategories();
        return categories.Select(c => new ServiceCategoryDTO
        {
            Id = c.Id,
            NamePl = c.NamePl,
            NameEn = c.NameEn
        }).ToList();
    }

    public async Task<ICollection<ServiceDTO>> GetAllServicesForReceptionistAsync(string lang)
    {
        return await _serviceRepository.GetAllServicesForReceptionistAsync(lang);
    }
}