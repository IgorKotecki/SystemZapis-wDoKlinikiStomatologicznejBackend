using SystemZapisowDoKlinikiApi.DTO.UserDtos;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface ITeamRepository
{
    public Task<ICollection<TeamDto>> GetAllTeamMembersAsync();
}