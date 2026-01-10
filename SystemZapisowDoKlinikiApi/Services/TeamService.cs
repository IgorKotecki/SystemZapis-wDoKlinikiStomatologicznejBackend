using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories;

namespace SystemZapisowDoKlinikiApi.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly string _cloudName;

    public TeamService(ITeamRepository teamRepository, IConfiguration configuration)
    {
        _teamRepository = teamRepository;
        _cloudName = configuration["Cloudinary:CloudName"]!;
    }

    public async Task<ICollection<TeamDTO>> GetAllTeamMembersAsync()
    {
        var members = await _teamRepository.GetAllTeamMembersAsync();
        /*foreach (var member in members)
        {
            if (!string.IsNullOrEmpty(member.PhotoURL))
            {
                member.PhotoURL =
                    $"https://res.cloudinary.com/{_cloudName}/image/upload/q_auto,f_auto/{member.PhotoURL}.jpg";
            }
        }*/

        return members;
    }
}