using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.DTO.TimeBlocksDtos;

namespace SystemZapisowDoKlinikiIntegrationTest;

[TestFixture]
public class TimeBlocksControllerIntegrationTests
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
    public async Task GetTimeBlocks_ShouldReturnTimeBlocks_WhenDoctorIdAndDateAreValid()
    {
        var doctorId = 6;
        var date = new DateRequest
        {
            Year = 2026,
            Month = 1,
            Day = 15
        };
        // Arrange


        // Act
        var response =
            await _client.GetAsync($"/api/time-blocks/{doctorId}?Year={date.Year}&Month={date.Month}&Day={date.Day}");
        var timeBlocks = await response.Content.ReadFromJsonAsync<List<TimeBlockDto>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(timeBlocks, Is.Not.Null);
    }

    [Test]
    public async Task AddWorkingHoursAsync_ShouldAddWorkingHours_WhenRequestIsValid()
    {
        // Arrange
        var workingHoursDto = new WorkingHoursDto
        {
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(2)
        };

        var content = JsonContent.Create(workingHoursDto);

        // Act
        var response = await _client.PostAsync("/api/time-blocks/working-hours", content);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }
}