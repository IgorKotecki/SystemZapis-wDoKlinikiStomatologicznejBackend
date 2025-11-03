using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("TeamMembers")]
    public async Task<ActionResult<ICollection<TeamDTO>>> GetAllTeamMembersAsync()
    {
        try
        {
            var teamMembers = await _teamService.GetAllTeamMembersAsync();
            if(!teamMembers.Any())
                return NotFound("No team members found.");
            return Ok(teamMembers);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}