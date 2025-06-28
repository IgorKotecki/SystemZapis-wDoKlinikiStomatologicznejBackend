using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
            .Where(s => s.Roles.Any(r => r.Name == "User" || r.Name == "Guest"))
            .Select(s => new ServiceDTO()
            {
                Id = s.Id,
                LowPrice = s.LowPrice,
                HighPrice = s.HighPrice,
                MinTime = s.MinTime,
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

    [Authorize(Roles = "Receptionist")]
    public async Task<ICollection<Service?>> GetAllServicesAsync()
    {
        return await _context.Services.ToListAsync();
    }
}