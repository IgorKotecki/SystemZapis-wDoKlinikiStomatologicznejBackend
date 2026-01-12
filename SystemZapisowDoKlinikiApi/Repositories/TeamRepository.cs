using Microsoft.EntityFrameworkCore;
using SystemZapisowDoKlinikiApi.Context;
using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ClinicDbContext _context;

    public TeamRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<TeamDto>> GetAllTeamMembersAsync()
    {
        var staffRoles = new int[] { 1, 2 };

        return await _context.Users
            .Where(u => staffRoles.Contains(u.RolesId))
            .Include(u => u.Doctor)
            .Include(u => u.Roles)
            .Select(u => new TeamDto
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                PhotoURL = u.PhotoURL,
                SpecializationPl = u.Doctor != null ? u.Doctor.SpecializationPl : null,
                SpecializationEn = u.Doctor != null ? u.Doctor.SpecializationEn : null,
                RoleName = u.Roles != null ? u.Roles.Name : null
            })
            .ToListAsync();
    }
}