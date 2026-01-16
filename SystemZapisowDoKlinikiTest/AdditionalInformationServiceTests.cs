using Moq;
using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiTest;

[TestFixture]
public class AdditionalInformationServiceTests
{
    private Mock<IAdditionalInformationRepository> _repositoryMock;
    private AdditionalInformationService _service;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IAdditionalInformationRepository>();
        _service = new AdditionalInformationService(_repositoryMock.Object);
    }

    [Test]
    public async Task GetAddInformationByIdAsync_ShouldCallRepository_WhenCalled()
    {
        // Arrange
        int id = 1;
        string lang = "en";
        var expected = new AddInformationOutDto
        {
            Id = 0,
            Body = null
        };

        _repositoryMock
            .Setup(repo => repo.GetAddInformationByIdAsync(id, lang))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetAddInformationByIdAsync(id, lang);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        _repositoryMock.Verify(repo => repo.GetAddInformationByIdAsync(id, lang), Times.Once);
    }

    [Test]
    public async Task DeleteAddInformationByIdAsync_ShouldCallRepository_WhenCalled()
    {
        // Arrange
        int id = 1;

        // Act
        await _service.DeleteAddInformationByIdAsync(id);

        // Assert
        _repositoryMock.Verify(repo => repo.DeleteAddInformationByIdAsync(id), Times.Once);
    }

    [Test]
    public void CreateAddInformationAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
        // Arrange
        AddInformationDto dto = null!;

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _service.CreateAddInformationAsync(dto));
        Assert.That(exception?.Message, Does.Contain("AddInformationDto cannot be null."));
    }

    [Test]
    public void CreateAddInformationAsync_ShouldThrowArgumentException_WhenBodyPlIsNullOrEmpty()
    {
        // Arrange
        var dto = new AddInformationDto
        {
            BodyPl = "",
            BodyEn = "Valid BodyEn",
            Language = "en"
        };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.CreateAddInformationAsync(dto));
        Assert.That(exception?.Message, Does.Contain("BodyPl cannot be null or empty."));
    }

    [Test]
    public void CreateAddInformationAsync_ShouldThrowArgumentException_WhenBodyEnIsNullOrEmpty()
    {
        // Arrange
        var dto = new AddInformationDto
        {
            BodyPl = "Valid BodyPl",
            BodyEn = "",
            Language = "pl"
        };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.CreateAddInformationAsync(dto));
        Assert.That(exception?.Message, Does.Contain("BodyEn cannot be null or empty."));
    }

    [Test]
    public async Task CreateAddInformationAsync_ShouldCallRepository_WhenDtoIsValid()
    {
        // Arrange
        var dto = new AddInformationDto
        {
            BodyPl = "Valid BodyPl",
            BodyEn = "Valid BodyEn",
            Language = null!
        };
        var expected = new AddInformationOutDto
        {
            Id = 0,
            Body = null!
        };

        _repositoryMock
            .Setup(repo => repo.CreateAddInformationAsync(dto))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.CreateAddInformationAsync(dto);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        _repositoryMock.Verify(repo => repo.CreateAddInformationAsync(dto), Times.Once);
    }

    [Test]
    public async Task GetAddInformationAsync_ShouldCallRepository_WhenCalled()
    {
        // Arrange
        string lang = "en";
        var expected = new List<AddInformationOutDto>();

        _repositoryMock
            .Setup(repo => repo.GetAddInformationAsync(lang))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetAddInformationAsync(lang);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
        _repositoryMock.Verify(repo => repo.GetAddInformationAsync(lang), Times.Once);
    }
}