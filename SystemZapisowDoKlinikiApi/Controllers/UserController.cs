using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound($"User with id = {id} not found.");
        }

        return Ok(user);
    }
}