using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) { 
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
    
    [HttpPut("edit/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO dto)
    {
        var updated = await _userService.UpdateUserAsync(id, dto);

        if (updated == null)
            return NotFound("User not found");

        return Ok(updated);
    }
}