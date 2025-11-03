using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ClinicDbContext _context;
    
    public TeamRepository(ClinicDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<TeamDTO>> GetAllTeamMembersAsync()
    {
        return await _context.Users
            .Where(u => u.RolesId == 1 || u.RolesId == 2)
            .Select(u => new TeamDTO()
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                SpecializationPl = u.Doctor != null ? u.Doctor.SpecializationPl : null,
                SpecializationEn = u.Doctor != null ? u.Doctor.SpecializationEn : null
            })
            .ToListAsync();
    }
}