using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories;

public interface ITeamRepository
{
    public Task<ICollection<TeamDTO>> GetAllTeamMembersAsync();
}