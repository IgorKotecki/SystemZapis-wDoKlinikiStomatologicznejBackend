using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ICollection<ServiceDto>> GetAllServicesAvailableForClientWithLangAsync(string lang)
    {
        ValidateLanguage(lang);

        var services = await _serviceRepository.GetAllServicesAvailableForClientWithLangAsync(lang);

        if (services == null || !services.Any())
        {
            throw new ArgumentException("No services found for the specified language.");
        }

        return services;
    }

    private void ValidateLanguage(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
        {
            throw new ArgumentException("Language code is required.");
        }
    }

    public async Task AddServiceAsync(AddServiceDto addServiceDto)
    {
        ValidateAddServiceDto(addServiceDto);

        await _serviceRepository.AddServiceAsync(addServiceDto);
    }

    private void ValidateAddServiceDto(AddServiceDto addServiceDto)
    {
        if (addServiceDto == null)
        {
            throw new ArgumentNullException(nameof(addServiceDto), "Service data is required.");
        }

        if (addServiceDto.LowPrice == null && addServiceDto.HighPrice == null)
        {
            throw new ArgumentException("At least one price (LowPrice or HighPrice) must be provided.");
        }
    }

    public async Task<AllServicesDto> GerAllServicesAsync(string lang)
    {
        lang = EnsureValidLanguage(lang);

        var services = await _serviceRepository.GetAllServicesAsync(lang);

        if (services.ServicesByCategory.IsNullOrEmpty())
        {
            throw new KeyNotFoundException("No services found.");
        }

        return services;
    }

    private string EnsureValidLanguage(string lang)
    {
        return string.IsNullOrEmpty(lang) ? "pl" : lang;
    }

    public async Task<Service?> GetServiceByIdAsync(int serviceId)
    {
        return await _serviceRepository.GetServiceByIdAsync(serviceId);
    }

    public async Task<ServiceEditDto?> GetServiceForEditAsync(int serviceId)
    {
        return await _serviceRepository.GetServiceEditDtoByIdAsync(serviceId);
    }


    public Task DeleteServiceAsync(int serviceId)
    {
        return _serviceRepository.DeleteServiceAsync(serviceId);
    }

    public Task EditServiceAsync(int serviceId, ServiceEditDto serviceEditDto)
    {
        if (serviceEditDto == null)
            throw new ArgumentNullException(nameof(serviceEditDto));

        return _serviceRepository.EditServiceAsync(serviceId, serviceEditDto);
    }


    public async Task<List<ServiceCategoryDto>> GetAllServiceCategories()
    {
        var categories = await _serviceRepository.GetAllServiceCategories();
        return categories.Select(c => new ServiceCategoryDto
        {
            Id = c.Id,
            NamePl = c.NamePl,
            NameEn = c.NameEn
        }).ToList();
    }

    public async Task<ICollection<ServiceDto>> GetAllServicesForReceptionistAsync(string lang)
    {
        return await _serviceRepository.GetAllServicesForReceptionistAsync(lang);
    }
}