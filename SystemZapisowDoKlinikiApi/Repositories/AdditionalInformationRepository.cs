using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.Context;
using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class AdditionalInformationRepository : IAdditionalInformationRepository
{
    private readonly ClinicDbContext _context;

    public AdditionalInformationRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<AddInformationOutDto> CreateAddInformationAsync(AddInformationDto addInformationDto)
    {
        var addInformation = new AdditionalInformation
        {
            BodyEn = addInformationDto.BodyEn,
            BodyPl = addInformationDto.BodyPl
        };
        _context.AdditionalInformations.Add(addInformation);

        await _context.SaveChangesAsync();

        return new AddInformationOutDto()
        {
            Id = addInformation.Id,
            Body = addInformationDto.Language == "pl" ? addInformation.BodyPl : addInformation.BodyEn
        };
    }

    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang)
    {
        var addInformation = await _context.AdditionalInformations.ToListAsync();
        var result = addInformation.Select(ai => new AddInformationOutDto
            {
                Id = ai.Id,
                Body = lang == "pl" ? ai.BodyPl : ai.BodyEn
            })
            .OrderBy(ad => ad.Body)
            .ToList();
        return result;
    }

    public Task<AddInformationOutDto> GetAddInformationByIdAsync(int id, string lang)
    {
        return _context.AdditionalInformations
            .Where(ai => ai.Id == id)
            .Select(ai => new AddInformationOutDto
            {
                Id = ai.Id,
                Body = lang == "pl" ? ai.BodyPl : ai.BodyEn
            })
            .FirstOrDefaultAsync()!;
    }

    public async Task DeleteAddInformationByIdAsync(int id)
    {
        var addInformation = await _context.AdditionalInformations
            .Include(ad => ad.Appointments)
            .FirstOrDefaultAsync(ad => ad.Id == id);

        if (addInformation == null)
        {
            throw new KeyNotFoundException($"Additional information with ID {id} not found.");
        }

        addInformation.Appointments.Clear();

        _context.AdditionalInformations.Remove(addInformation);

        await _context.SaveChangesAsync();
    }
}