using Moq;
using SystemZapisowDoKlinikiApi.DTO.ToothDtos;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiTest;

[TestFixture]
public class ToothServiceTests
{
    private Mock<IToothRepository> _toothRepositoryMock;
    private ToothService _toothService;

    [SetUp]
    public void SetUp()
    {
        _toothRepositoryMock = new Mock<IToothRepository>();
        _toothService = new ToothService(_toothRepositoryMock.Object);
    }

    [Test]
    public async Task GetToothStatusesAsync_ShouldReturnToothStatuses_WhenLanguageIsValid()
    {
        // Arrange
        string language = "en";
        var expectedStatuses = new ToothStatusesDto();

        _toothRepositoryMock
            .Setup(repo => repo.GetToothStatusesAsync(language))
            .ReturnsAsync(expectedStatuses);

        // Act
        var result = await _toothService.GetToothStatusesAsync(language);

        // Assert
        Assert.That(result, Is.EqualTo(expectedStatuses));
        _toothRepositoryMock.Verify(repo => repo.GetToothStatusesAsync(language), Times.Once);
    }

    [Test]
    public void GetToothStatusesAsync_ShouldThrowArgumentException_WhenLanguageIsNullOrEmpty()
    {
        // Arrange
        string language = "";

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _toothService.GetToothStatusesAsync(language));
        Assert.That(exception?.Message, Is.EqualTo("Language parameter is required (Parameter 'language')"));
    }

    [Test]
    public void GetToothStatusesAsync_ShouldThrowArgumentException_WhenLanguageIsUnsupported()
    {
        // Arrange
        string language = "fr";

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _toothService.GetToothStatusesAsync(language));
        Assert.That(exception?.Message,
            Is.EqualTo("Unsupported language code. Supported codes are 'pl' and 'en'. (Parameter 'language')"));
    }
}