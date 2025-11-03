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
    
    public Task<ICollection<TeamDTO>> GetAllTeamMembersAsync()
    {
        return _teamRepository.GetAllTeamMembersAsync();
    }
}