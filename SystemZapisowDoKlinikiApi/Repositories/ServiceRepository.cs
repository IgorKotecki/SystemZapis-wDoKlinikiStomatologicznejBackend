using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly ClinicDbContext _context;

    public ServiceRepository(ClinicDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<ICollection<ServiceDTO>> GetAllServicesAvailableForClientWithLangAsync(string lang)
    {
        Console.WriteLine($"Fetching services for language: {lang}");
        return await _context.Services
            .Where(s => s.Roles.Any(r => r.Name == "Registered_user" || r.Name == "Unregistered_user"))
            .Select(s => new ServiceDTO()
            {
                Id = s.Id,
                LowPrice = s.LowPrice,
                HighPrice = s.HighPrice,
                MinTime = s.MinTime,
                //Category = s.Category,
                LanguageCode = lang,
                Name = s.ServicesTranslations.FirstOrDefault(st => st.LanguageCode == lang)!.Name,
                Description = s.ServicesTranslations.FirstOrDefault(st => st.LanguageCode == lang)!.Description
            })
            .ToListAsync();
    }

    public async Task<Service?> GetServiceByIdAsync(int serviceId)
    {
        return await _context.Services.FirstOrDefaultAsync(s => s.Id == serviceId);
    }

    public async Task AddServiceAsync(AddServiceDto addServiceDto)
    {
        var trasaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var requiredService = await _context.Services.FirstOrDefaultAsync(s => s.Id == addServiceDto.serviceId);

            if (requiredService == null && addServiceDto.serviceId != 0)
            {
                throw new ArgumentException("Required service with given id does not exist");
            }
            
            var roles = await _context.Roles
                .Where(r => addServiceDto.rolePermissionIds.Contains(r.Id))
                .ToListAsync();

            if (roles.IsNullOrEmpty())
            {
                throw new Exception("No valid roles found for the provided role IDs");
            }
            
            var newService = new Service()
            {
                LowPrice = addServiceDto.LowPrice,
                HighPrice = addServiceDto.HighPrice,
                MinTime = addServiceDto.MinTime,
                Roles = roles
            };

            await _context.Services.AddAsync(newService);
            await _context.SaveChangesAsync();

            var newServiceId = newService.Id;

            foreach (var language in addServiceDto.Languages)
            {
                var serviceTranslation = new ServicesTranslation()
                {
                    ServiceId = newServiceId,
                    LanguageCode = language.Code,
                    Name = language.Name,
                    Description = language.Description
                };
                await _context.ServicesTranslations.AddAsync(serviceTranslation);
            }

            await _context.SaveChangesAsync();

            var newServiceDependencies = new ServiceDependency()
            {
                RequiredService = requiredService,
                Service = newService
            };

            await _context.ServiceDependencies.AddAsync(newServiceDependencies);
            await _context.SaveChangesAsync();

            await trasaction.CommitAsync();
        }
        catch (Exception e)
        {
            await trasaction.RollbackAsync();
            throw new Exception("Failed to add service", e);
        }
    }

    public async Task<ICollection<ServiceDTO>> GetAllServicesAsync(string lang)
    {
        return await _context.Services
            .Select(s => new ServiceDTO()
            {
                Id = s.Id,
                LowPrice = s.LowPrice,
                HighPrice = s.HighPrice,
                MinTime = s.MinTime,
                Name = s.ServicesTranslations
                    .Where(st => st.LanguageCode == lang)
                    .Select(st => st.Name)
                    .FirstOrDefault(),
                Description = s.ServicesTranslations
                    .Where(st => st.LanguageCode == lang)
                    .Select(st => st.Description)
                    .FirstOrDefault(),
                LanguageCode = lang
            })
            .ToListAsync();
    }

    [Authorize(Roles = "Receptionist")]
    public async Task<ICollection<Service?>> GetAllServicesAsync()
    {
        return await _context.Services.ToListAsync();
    }
}