using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiIntegrationTest;

[TestFixture]
public class ServiceRepositoryIntegrationTests
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
    public async Task GetAllServicesLangEn_ShouldReturnServicesList()
    {
        // Act
        var response = await _client.GetAsync("/api/Service/services?lang=en");
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        var services = await response.Content.ReadFromJsonAsync<ServiceDto>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(services, Is.Not.Null);
    }

    [Test]
    public async Task GetServiceForUsersLangEn_ShouldReturnServicesForUser()
    {
        // Act
        var response = await _client.GetAsync($"/api/Service/user-services?lang=en");
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        var service = await response.Content.ReadFromJsonAsync<ICollection<ServiceDto>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(service, Is.Not.Empty);
    }

    [Test]
    public async Task GetServicesCategories_ShouldReturnCategoriesList()
    {
        // Act
        var response = await _client.GetAsync("/api/Service/serviceCategories");
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        var categories = await response.Content.ReadFromJsonAsync<List<ServiceCategoryDto>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.That(categories, Is.Not.Null);
        Assert.That(categories!.Count, Is.GreaterThan(0));
    }
}