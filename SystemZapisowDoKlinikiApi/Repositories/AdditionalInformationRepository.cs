using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.Controllers;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Services;

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
            Body = addInformationDto.language == "pl" ? addInformation.BodyPl : addInformation.BodyEn
        };
    }

    public async Task<ICollection<AddInformationOutDto>> GetAddInformationAsync(string lang)
    {
        var addInformations = await _context.AdditionalInformations.ToListAsync();
        var result = addInformations.Select(ai => new AddInformationOutDto
        {
            Id = ai.Id,
            Body = lang == "pl" ? ai.BodyPl : ai.BodyEn
        }).ToList();
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
        var addInformation = await _context.AdditionalInformations.FindAsync(id);
        if (addInformation != null) _context.AdditionalInformations.Remove(addInformation);
        await _context.SaveChangesAsync();
    }
}