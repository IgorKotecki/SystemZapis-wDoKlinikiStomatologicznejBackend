using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services;

public interface ITeamService
{
    public Task<ICollection<TeamDTO>> GetAllTeamMembersAsync();
}