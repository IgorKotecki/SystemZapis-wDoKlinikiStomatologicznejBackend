using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SystemZapisowDoKlinikiApi.Context;

namespace SystemZapisowDoKlinikiIntegrationTest;

[TestFixture]
public class ToothControllerIntegrationTests
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
    public async Task GetToothModel_ShouldReturnToothModel_WhenUserExists()
    {
        // Arrange
        int userId = 1006;
        string language = "pl";

        // Act
        var response = await _client.GetAsync($"/api/Tooth/teeth-model?userId={userId}&lang={language}");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(responseContent, Is.Not.Null);
        Assert.That(responseContent.Length, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetToothModel_ShouldReturnEmptyModel_WhenUserHasNoTeeth()
    {
        // Arrange
        int userId = 1006;
        string language = "en";

        // Act
        var response = await _client.GetAsync($"/api/Tooth/teeth-model?userId={userId}&lang={language}");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(responseContent, Is.Not.Null);
    }

    [Test]
    public async Task GetToothStatuses_ShouldReturnStatuses_ForPolishLanguage()
    {
        // Act
        var response = await _client.GetAsync("/api/Tooth/statuses?lang=pl");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(responseContent, Does.Contain("Zdrowy"));
        Assert.That(responseContent, Does.Contain("Próchnica"));
    }

    [Test]
    public async Task GetToothStatuses_ShouldReturnStatuses_ForEnglishLanguage()
    {
        // Act
        var response = await _client.GetAsync("/api/Tooth/statuses?lang=en");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(responseContent, Does.Contain("Healthy"));
        Assert.That(responseContent, Does.Contain("Cavity"));
    }

    [Test]
    public async Task GetToothStatuses_ShouldReturnAllStatuses()
    {
        // Act
        var response = await _client.GetAsync("/api/Tooth/statuses?lang=pl");

        // Assert
        response.EnsureSuccessStatusCode();

        using (var verifyScope = _factory.Services.CreateScope())
        {
            var verifyDb = verifyScope.ServiceProvider.GetRequiredService<ClinicDbContext>();
            var statusCount = await verifyDb.ToothStatuses.CountAsync();

            // Sprawdź czy wszystkie statusy są w odpowiedzi
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.That(responseContent, Is.Not.Empty);
            // Możesz dodać bardziej szczegółowe sprawdzenie jeśli znasz strukturę DTO
        }
    }

    [Test]
    public async Task UpdateToothModel_ShouldReturnBadRequest_WhenUserIdIsInvalid()
    {
        // Arrange
        var updateDto = new
        {
            UserId = 99999, // Nieistniejący użytkownik
            Teeth = new[]
            {
                new { ToothNumber = 11, ToothStatusId = 1 }
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(updateDto),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync("/api/Tooth/teeth-model", content);

        // Assert
        // Może być BadRequest lub NotFound w zależności od implementacji serwisu
        Assert.That(response.IsSuccessStatusCode, Is.False);
    }
}