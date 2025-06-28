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
}