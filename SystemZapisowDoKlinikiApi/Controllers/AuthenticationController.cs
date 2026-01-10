using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjektSemestralnyTinWebApi.Security;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;
using RegisterRequest = SystemZapisowDoKlinikiApi.Models.RegisterRequest;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ClinicDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailSender;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ClinicDbContext context, IConfiguration configuration, IEmailService emailSender,
        ILogger<AuthenticationController> logger)
    {
        _context = context;
        _configuration = configuration;
        _emailSender = emailSender;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterRequest model)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest();
        }

        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(model.Password);

        var us = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (us != null)
        {
            if (us.Password != null)
            {
                return Conflict("User with this email already exists.");
            }
            else
            {
                us.Name = model.Name;
                us.Surname = model.Surname;
                us.PhoneNumber = model.PhoneNumber;
                us.Password = hashedPasswordAndSalt.Item1;
                us.Salt = hashedPasswordAndSalt.Item2;
                us.RefreshToken = SecurityHelper.GenerateRefreshToken();
                us.RefreshTokenExpDate = DateTime.UtcNow.AddDays(1);
                us.RolesId = 3; // Default role: Registered_User

                await _context.SaveChangesAsync();

                _logger.LogInformation("New user registered with id: {userId} and email: {email}", us.Id, us.Email);

                var userId = us.Id;

                await _context.Database.ExecuteSqlRawAsync("EXEC CreateDefaultTeethModelForUser @UserId = {0}", userId);

                return Ok();
            }
        }
        else
        {
            var user = new User()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = hashedPasswordAndSalt.Item1,
                Salt = hashedPasswordAndSalt.Item2,
                RefreshToken = SecurityHelper.GenerateRefreshToken(),
                RefreshTokenExpDate = DateTime.UtcNow.AddDays(1),
                RolesId = 3 // Default role: Registered_User
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var userId = user.Id;

            await _context.Database.ExecuteSqlRawAsync("EXEC CreateDefaultTeethModelForUser @UserId = {0}", userId);

            _logger.LogInformation("New user registered with id: {userId} and email: {email}", userId, user.Email);

            return Ok();
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(LoginRequest model)
    {
        var user = await _context.Users.Include(user => user.Roles).FirstOrDefaultAsync(u => u.Email == model.Email);

        if (ValidateUser(user))
        {
            return Unauthorized();
        }

        var hashedPassword = SecurityHelper.GetHashedPasswordWithSalt(model.Password, user!.Salt!);
        if (user.Password != hashedPassword)
        {
            return Unauthorized();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Roles.Name!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: credentials
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExpDate = DateTime.UtcNow.AddDays(1);

        await _context.SaveChangesAsync();

        _logger.LogInformation("User logged in with id: {userId} and email: {email}", user.Id, user.Email);

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken,
            photoURL = user.PhotoURL
        });
    }

    private bool ValidateUser(User? user)
    {
        if (user == null)
        {
            return true;
        }

        if (user.Salt == null)
        {
            return true;
        }

        if (user.Email == null)
        {
            return true;
        }

        if (user.Roles.Name == null)
        {
            return true;
        }

        return false;
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest model)
    {
        var user = await _context.Users.Include(user => user.Roles)
            .FirstOrDefaultAsync(u => u.RefreshToken!.Trim() == model.RefreshToken.Trim());

        if (ValidateUser(user))
        {
            return Unauthorized();
        }

        if (user!.RefreshTokenExpDate < DateTime.UtcNow)
        {
            return Unauthorized();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Roles.Name!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExpDate = DateTime.UtcNow.AddDays(1);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Refresh token issued for user with id: {userId} and email: {email}", user.Id,
            user.Email);

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
        {
            return BadRequest();
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Reset"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user!.Email!),
            new Claim("purpose", "passwordReset")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        var resetToken = new JwtSecurityTokenHandler().WriteToken(token);

        var resetLink = $"{_configuration["Frontend:Url"]}/resetPassword?token={resetToken}&email={user.Email}";

        await _emailSender.SendEmailAsync(model.Email, "Password Reset",
            $"Click the following link to reset your password: {resetLink}");

        _logger.LogInformation("Password reset link sent to email: {email}", user.Email);

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Reset"]!));
            tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out validatedToken);
        }
        catch
        {
            return Unauthorized();
        }

        var jwtToken = (JwtSecurityToken)validatedToken;
        var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (email == null)
        {
            return Unauthorized();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return BadRequest();
        }

        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(model.NewPassword);
        user.Password = hashedPasswordAndSalt.Item1;
        user.Salt = hashedPasswordAndSalt.Item2;
        user.RefreshToken = null;
        user.RefreshTokenExpDate = null;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Password reset for user with email: {email}", user.Email);

        return Ok();
    }
}