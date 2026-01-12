using SystemZapisowDoKlinikiApi.DTO.UserDtos;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface ITeamService
{
    public Task<ICollection<TeamDto>> GetAllTeamMembersAsync();
}