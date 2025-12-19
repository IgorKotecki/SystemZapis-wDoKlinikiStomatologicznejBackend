using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly ILogger<TeamController> _logger;

    public TeamController(ITeamService teamService, ILogger<TeamController> logger)
    {
        _teamService = teamService;
        _logger = logger;
    }

    [HttpGet("members")]
    public async Task<ActionResult<ICollection<TeamDTO>>> GetAllTeamMembersAsync()
    {
        var teamMembers = await _teamService.GetAllTeamMembersAsync();

        _logger.LogInformation("Retrieved {Count} team members.", teamMembers.Count);

        return Ok(teamMembers);
    }
}