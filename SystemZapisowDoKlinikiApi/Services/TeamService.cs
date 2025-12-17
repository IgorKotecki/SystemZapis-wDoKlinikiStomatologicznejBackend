using Microsoft.IdentityModel.Tokens;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;

    public TeamService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<ICollection<TeamDTO>> GetAllTeamMembersAsync()
    {
        var members = await _teamRepository.GetAllTeamMembersAsync();
        if (members.IsNullOrEmpty())
        {
            throw new KeyNotFoundException("No team members found.");
        }

        return members;
    }
}