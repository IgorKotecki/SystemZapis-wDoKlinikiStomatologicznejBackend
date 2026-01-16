using Moq;
using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiTest;

[TestFixture]
public class ServiceServiceTests
{
    private Mock<IServiceRepository> _repositoryMock;
    private ServiceService _service;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IServiceRepository>();
        _service = new ServiceService(_repositoryMock.Object);
    }

    [Test]
    public async Task GetAllServicesAvailableForClientWithLangAsync_ShouldCallRepository_WhenLangIsValid()
    {
        // Arrange
        string lang = "en";
        var expectedServices = new List<ServiceDto> { new ServiceDto() };

        _repositoryMock
            .Setup(repo => repo.GetAllServicesAvailableForClientWithLangAsync(lang))
            .ReturnsAsync(expectedServices);

        // Act
        var result = await _service.GetAllServicesAvailableForClientWithLangAsync(lang);

        // Assert
        Assert.That(result, Is.EqualTo(expectedServices));
        _repositoryMock.Verify(repo => repo.GetAllServicesAvailableForClientWithLangAsync(lang), Times.Once);
    }

    [Test]
    public void GetAllServicesAvailableForClientWithLangAsync_ShouldThrowArgumentException_WhenLangIsInvalid()
    {
        // Arrange
        string lang = "";

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.GetAllServicesAvailableForClientWithLangAsync(lang));
        Assert.That(exception?.Message, Does.Contain("Language code is required."));
    }

    [Test]
    public async Task AddServiceAsync_ShouldCallRepository_WhenDtoIsValid()
    {
        // Arrange
        var dto = new AddServiceDto
        {
            LowPrice = 100,
            HighPrice = 200,
            MinTime = 0,
            Languages = null!
        };

        // Act
        await _service.AddServiceAsync(dto);

        // Assert
        _repositoryMock.Verify(repo => repo.AddServiceAsync(dto), Times.Once);
    }

    [Test]
    public void AddServiceAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
        // Arrange
        AddServiceDto dto = null!;

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _service.AddServiceAsync(dto));
        Assert.That(exception?.Message, Does.Contain("Service data is required."));
    }

    [Test]
    public void AddServiceAsync_ShouldThrowArgumentException_WhenPricesAreNull()
    {
        // Arrange
        var dto = new AddServiceDto
        {
            LowPrice = null,
            HighPrice = null,
            MinTime = 0,
            Languages = null!
        };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.AddServiceAsync(dto));
        Assert.That(exception?.Message, Does.Contain("At least one price (LowPrice or HighPrice) must be provided."));
    }

    [Test]
    public async Task GetServiceByIdAsync_ShouldCallRepository_WhenCalled()
    {
        // Arrange
        int serviceId = 1;
        var expectedService = new Service();

        _repositoryMock
            .Setup(repo => repo.GetServiceByIdAsync(serviceId))
            .ReturnsAsync(expectedService);

        // Act
        var result = await _service.GetServiceByIdAsync(serviceId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedService));
        _repositoryMock.Verify(repo => repo.GetServiceByIdAsync(serviceId), Times.Once);
    }

    [Test]
    public async Task DeleteServiceAsync_ShouldCallRepository_WhenCalled()
    {
        // Arrange
        int serviceId = 1;

        // Act
        await _service.DeleteServiceAsync(serviceId);

        // Assert
        _repositoryMock.Verify(repo => repo.DeleteServiceAsync(serviceId), Times.Once);
    }
}