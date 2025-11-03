using SystemZapisowDoKlinikiApi.DTO;
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
        //TODO
        
        await _serviceRepository.AddServiceAsync(addServiceDto);
    }

    public Task<ICollection<ServiceDTO>> GerAllServicesAsync(string lang)
    {
        if (string.IsNullOrEmpty(lang))
            lang = "pl";

        return _serviceRepository.GetAllServicesAsync(lang);
    }
}