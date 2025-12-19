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
                LanguageCode = lang,
                PhotoUrl = s.PhotoUrl,
                Name = s.ServicesTranslations.FirstOrDefault(st => st.LanguageCode == lang)!.Name,
                Description = s.ServicesTranslations.FirstOrDefault(st => st.LanguageCode == lang)!.Description
            })
            .OrderBy(sdto => sdto.Name)
            .ToListAsync();
    }

    public async Task<Service?> GetServiceByIdAsync(int serviceId)
    {
        return await _context.Services
            .FirstOrDefaultAsync(s => s.Id == serviceId);
    }

    public async Task<ServiceEditDTO?> GetServiceEditDTOByIdAsync(int serviceId)
    {
        return await _context.Services
            .Where(s => s.Id == serviceId)
            .Select(s => new ServiceEditDTO
            {
                Id = s.Id,
                LowPrice = s.LowPrice,
                HighPrice = s.HighPrice,
                MinTime = s.MinTime,

                NamePl = s.ServicesTranslations
                    .Where(t => t.LanguageCode == "pl")
                    .Select(t => t.Name)
                    .FirstOrDefault() ?? "",

                DescriptionPl = s.ServicesTranslations
                    .Where(t => t.LanguageCode == "pl")
                    .Select(t => t.Description)
                    .FirstOrDefault() ?? "",

                NameEn = s.ServicesTranslations
                    .Where(t => t.LanguageCode == "en")
                    .Select(t => t.Name)
                    .FirstOrDefault() ?? "",

                DescriptionEn = s.ServicesTranslations
                    .Where(t => t.LanguageCode == "en")
                    .Select(t => t.Description)
                    .FirstOrDefault() ?? "",

                ServiceCategoryIds = s.ServiceCategories
                    .Select(sc => sc.Id)
                    .ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task AddServiceAsync(AddServiceDto addServiceDto)
    {
        var trasaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var requiredService = await _context.Services.FirstOrDefaultAsync(s => s.Id == addServiceDto.serviceId);

            if (requiredService == null && (addServiceDto.serviceId != 0 && addServiceDto.serviceId != null))
            {
                throw new ArgumentException("Required service with given id does not exist");
            }

            List<Role> roles;
            if (!addServiceDto.rolePermissionIds.IsNullOrEmpty())
            {
                roles = await _context.Roles
                    .Where(r => addServiceDto.rolePermissionIds.Contains(r.Id))
                    .ToListAsync();
                if (roles.IsNullOrEmpty())
                {
                    throw new Exception("No valid roles found for the provided role IDs");
                }
            }
            else
            {
                roles = new List<Role>();
            }

            var newService = new Service()
            {
                LowPrice = addServiceDto.LowPrice,
                HighPrice = addServiceDto.HighPrice,
                MinTime = addServiceDto.MinTime,
                Roles = roles,
                ServiceCategories = await _context.ServiceCategory
                    .Where(sc => addServiceDto.ServiceCategoriesId.Contains(sc.Id))
                    .ToListAsync()
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

    public async Task<AllServicesDto> GetAllServicesAsync(string lang)
    {
        var serviceDtos = await _context.Services
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
                LanguageCode = lang,
                Catergories = s.ServiceCategories
                    .Select(sc => lang == "pl" ? sc.NamePl : sc.NameEn)
                    .ToList()
            }).OrderBy(s => s.Id)
            .ToListAsync();
        var servicesByCategory = new Dictionary<string, ICollection<ServiceDTO>>();
        foreach (var serviceDto in serviceDtos)
        {
            foreach (var category in serviceDto.Catergories)
            {
                if (!servicesByCategory.ContainsKey(category))
                {
                    servicesByCategory[category] = new List<ServiceDTO>();
                }

                servicesByCategory[category].Add(serviceDto);
            }
        }

        return new AllServicesDto
        {
            ServicesByCategory = servicesByCategory
        };
    }

    public async Task DeleteServiceAsync(int serviceId)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var serviceToDelete = await _context.Services.FirstOrDefaultAsync(s => s.Id == serviceId);
            if (serviceToDelete == null)
            {
                throw new ArgumentException("Service with given id does not exist");
            }

            await _context.ServiceDependencies.Where(s => s.ServiceId == serviceId || s.RequiredServiceId == serviceId)
                .ForEachAsync(sd => _context.ServiceDependencies.Remove(sd));

            _context.Services.Remove(serviceToDelete);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception("Failed to delete service", e);
        }
    }

    public async Task EditServiceAsync(int serviceId, ServiceEditDTO serviceEditDto)
    {
        var service = await _context.Services
            .Include(s => s.ServicesTranslations)
            .Include(s => s.ServiceCategories)
            .FirstOrDefaultAsync(s => s.Id == serviceId);

        if (service == null)
            throw new ArgumentException("Service not found");
        service.LowPrice = serviceEditDto.LowPrice;
        service.HighPrice = serviceEditDto.HighPrice;
        service.MinTime = serviceEditDto.MinTime;

        UpdateTranslation(service, "pl", serviceEditDto.NamePl, serviceEditDto.DescriptionPl);
        UpdateTranslation(service, "en", serviceEditDto.NameEn, serviceEditDto.DescriptionEn);

        service.ServiceCategories.Clear();

        if (serviceEditDto.ServiceCategoryIds.Any())
        {
            var categories = await _context.ServiceCategory
                .Where(c => serviceEditDto.ServiceCategoryIds.Contains(c.Id))
                .ToListAsync();

            foreach (var category in categories)
            {
                service.ServiceCategories.Add(category);
            }
        }

        await _context.SaveChangesAsync();
    }

    private static void UpdateTranslation(
        Service service,
        string lang,
        string name,
        string description)
    {
        var translation = service.ServicesTranslations
            .FirstOrDefault(t => t.LanguageCode == lang);

        if (translation == null)
        {
            service.ServicesTranslations.Add(new ServicesTranslation
            {
                LanguageCode = lang,
                Name = name,
                Description = description
            });
        }
        else
        {
            translation.Name = name;
            translation.Description = description;
        }
    }


    public async Task<List<ServiceCategory>> GetAllServiceCategories()
    {
        return await _context.ServiceCategory.AsNoTracking().ToListAsync();
    }

    public async Task<ICollection<ServiceDTO>> GetAllServicesForReceptionistAsync(string lang)
    {
        return await _context.Services
            .Select(s => new ServiceDTO()
            {
                Id = s.Id,
                LowPrice = s.LowPrice,
                HighPrice = s.HighPrice,
                MinTime = s.MinTime,
                LanguageCode = lang,
                PhotoUrl = s.PhotoUrl,
                Name = s.ServicesTranslations.FirstOrDefault(st => st.LanguageCode == lang)!.Name,
                Description = s.ServicesTranslations.FirstOrDefault(st => st.LanguageCode == lang)!.Description
            })
            .OrderBy(sdto => sdto.Name)
            .ToListAsync();
    }

    public async Task<ICollection<Service?>> GetAllServicesAsync()
    {
        return await _context.Services.ToListAsync();
    }
}