using Microsoft.AspNetCore.Mvc.Testing;

namespace SystemZapisowDoKlinikiIntegrationTest;

[TestFixture]
public class TeamRepositoryTests
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
    public async Task GetTeams_ShouldReturnTeamsList()
    {
        // Act
        var response = await _client.GetAsync("/api/Team/members");
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(responseContent, Is.Not.Null);
        Assert.That(responseContent.Length, Is.GreaterThan(0));
    }
}