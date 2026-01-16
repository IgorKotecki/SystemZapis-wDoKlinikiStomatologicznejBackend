using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SystemZapisowDoKlinikiApi.Context;
using SystemZapisowDoKlinikiApi.Models;

namespace SystemZapisowDoKlinikiIntegrationTest;

[TestFixture]
public class UserControllerIntegrationTests
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
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        int userId = 1006;

        // Act
        var response = await _client.GetAsync($"/api/User/{userId}");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task UpdateUser_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        int userId;
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();

            var user = new User
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                RolesId = 4
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            userId = user.Id;
        }

        var updateDto = new
        {
            Name = "Jane",
            Surname = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "987654321"
        };
        var content = new StringContent(
            JsonSerializer.Serialize(updateDto),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/api/User/edit/{userId}", content);

        // Assert
        response.EnsureSuccessStatusCode();

        // Verify update
        using (var verifyScope = _factory.Services.CreateScope())
        {
            var verifyContext = verifyScope.ServiceProvider.GetRequiredService<ClinicDbContext>();
            var updatedUser = await verifyContext.Users.FindAsync(userId);

            Assert.NotNull(updatedUser);
            Assert.That(updatedUser!.Name, Is.EqualTo("Jane"));
            Assert.That(updatedUser.Surname, Is.EqualTo("Smith"));
            Assert.That(updatedUser.Email, Is.EqualTo("jane.smith@example.com"));
        }
    }

    [Test]
    public async Task DeleteUser_ShouldRemoveUser_WhenUserExists()
    {
        // Arrange
        int userId;
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();

            var user = new User
            {
                Name = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                RolesId = 4
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            userId = user.Id;
        }

        // Act
        var response = await _client.DeleteAsync($"/api/User/delete/{userId}");

        // Assert
        response.EnsureSuccessStatusCode();

        // Verify deletion
        using (var verifyScope = _factory.Services.CreateScope())
        {
            var verifyContext = verifyScope.ServiceProvider.GetRequiredService<ClinicDbContext>();
            var deletedUser = await verifyContext.Users.FindAsync(userId);

            Assert.Null(deletedUser);
        }
    }

    [Test]
    public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/User/99999");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
    }
}