using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound($"User with id = {id} not found.");
        }

        _logger.LogInformation("Retrieved user with id: {UserId}", id);

        return Ok(user);
    }

    [HttpPut("edit/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
    {
        if (ModelState.IsValid == false)
            return BadRequest(ModelState);

        var updated = await _userService.UpdateUserAsync(id, dto);

        if (updated == null)
            return NotFound("User not found");

        _logger.LogInformation("Updated user with id: {UserId}", id);

        return Ok(updated);
    }

    [HttpGet("all-users")]
    [Authorize(Roles = "Admin,Receptionist,Doctor")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page = 0, [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var users = await _userService.GetAllUsersAsync(page, pageSize, searchTerm);

        _logger.LogInformation("Retrieved all users.");

        return Ok(users);
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _userService.DeleteUserAsync(userId);

        _logger.LogInformation("Deleted user with id: {UserId}", userId);

        return NoContent();
    }
}