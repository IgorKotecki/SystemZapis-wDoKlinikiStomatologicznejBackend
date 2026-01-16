using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SystemZapisowDoKlinikiApi.Context;
using SystemZapisowDoKlinikiApi.DTO.AuthDtos;
using RegisterRequest = SystemZapisowDoKlinikiApi.DTO.AuthDtos.RegisterRequest;

namespace SystemZapisowDoKlinikiIntegrationTest;

[TestFixture]
public class AuthenticationControllerIntegrationTests
{
    private HttpClient _client;
    private static WebApplicationFactory<Program> _factory;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task RegisterUserAsync_ShouldReturnOk_WhenDataIsValid()
    {
        // Arrange
        var registerDto = new RegisterRequest
        {
            Email = "test.user3@example.com",
            Password = "StrongP@ssw0rd",
            Name = "Test",
            Surname = "User",
            PhoneNumber = "123456789"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(registerDto),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/auth/register", content);


        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task LoginUserAsync_ShouldReturnTokens_WhenDataValid()
    {
        var loginRequest = new LoginRequest()
        {
            Email = "paweszeliga@gmail.com",
            Password = "12341234"
        };
        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.That(responseContent, Does.Contain("accessToken"));
        Assert.That(responseContent, Does.Contain("refreshToken"));
    }

    [Test]
    public async Task RefreshTokenAsync_ShouldReturnNewToken_WhenDataValid()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == 1006);
        var refreshToken = user?.RefreshToken;

        var refreshRequest = new RefreshTokenRequest()
        {
            RefreshToken = refreshToken
        };

        var content = new StringContent(
            JsonSerializer.Serialize(refreshRequest),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/auth/refresh", content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.That(responseContent, Does.Contain("accessToken"));
    }
}