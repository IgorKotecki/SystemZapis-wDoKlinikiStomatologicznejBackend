using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

namespace SystemZapisowDoKlinikiIntegrationTest;

[TestFixture]
public class AppointmentIntegrationTests
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
    public async Task BookGuestAppointmentAsync_ShouldReturnSuccess_WhenRequestIsValid()
    {
        // Arrange
        var appointmentRequest = new AppointmentRequest
        {
            Name = "Jan",
            Surname = "Kowalski",
            DoctorId = 6,
            Duration = 1,
            StartTime = new DateTime(2026, 2, 20, 13, 0, 0),
            Email = "jankowalski@gmail.com",
            PhoneNumber = "123456789",
            ServicesIds = new List<int>() { 1002 }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(appointmentRequest),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/appointment/guest/appointment", content);

        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");
        // Assert
        response.EnsureSuccessStatusCode();
    }
}