using Moq;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.DTO.TimeBlocksDtos;
using SystemZapisowDoKlinikiApi.Exceptions;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiTest;

[TestFixture]
public class TimeBlockServiceTests
{
    private Mock<ITimeBlockRepository> _timeBlockRepositoryMock;
    private TimeBlockService _timeBlockService;

    [SetUp]
    public void SetUp()
    {
        _timeBlockRepositoryMock = new Mock<ITimeBlockRepository>();
        _timeBlockService = new TimeBlockService(_timeBlockRepositoryMock.Object);
    }

    [Test]
    public async Task GetTimeBlocksAsync_ShouldReturnTimeBlocks_WhenTimeBlocksExist()
    {
        // Arrange
        int doctorId = 1;
        var date = new DateRequest();
        var expectedTimeBlocks = new List<TimeBlockDto>
        {
            new TimeBlockDto
            {
                User = null!
            }
        };

        _timeBlockRepositoryMock
            .Setup(repo => repo.GetTimeBlocksAsync(doctorId, date))
            .ReturnsAsync(expectedTimeBlocks);

        // Act
        var result = await _timeBlockService.GetTimeBlocksAsync(doctorId, date);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTimeBlocks));
        _timeBlockRepositoryMock.Verify(repo => repo.GetTimeBlocksAsync(doctorId, date), Times.Once);
    }

    [Test]
    public void GetTimeBlocksAsync_ShouldThrowKeyNotFoundException_WhenNoTimeBlocksExist()
    {
        // Arrange
        int doctorId = 1;
        var date = new DateRequest();

        _timeBlockRepositoryMock
            .Setup(repo => repo.GetTimeBlocksAsync(doctorId, date))
            .ReturnsAsync(new List<TimeBlockDto>());

        // Act & Assert
        var exception = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _timeBlockService.GetTimeBlocksAsync(doctorId, date));
        Assert.That(exception?.Message, Is.EqualTo("No time blocks found for the given doctor and date."));
    }

    [Test]
    public async Task GetTimeBlockByDoctorBlockIdAsync_ShouldReturnTimeBlock_WhenTimeBlockExists()
    {
        // Arrange
        int blockId = 1;
        var expectedTimeBlock = new TimeBlockDto
        {
            User = null!
        };

        _timeBlockRepositoryMock
            .Setup(repo => repo.GetTimeBlockByDoctorBlockIdAsync(blockId))
            .ReturnsAsync(expectedTimeBlock);

        // Act
        var result = await _timeBlockService.GetTimeBlockByDoctorBlockIdAsync(blockId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTimeBlock));
        _timeBlockRepositoryMock.Verify(repo => repo.GetTimeBlockByDoctorBlockIdAsync(blockId), Times.Once);
    }

    [Test]
    public async Task CheckIfAvailableTimeBlockAsync_ShouldReturnTrue_WhenTimeBlockIsAvailable()
    {
        // Arrange
        DateTime startTime = DateTime.Now.AddHours(1);
        int duration = 30;

        _timeBlockRepositoryMock
            .Setup(repo => repo.CheckIfAvailableTimeBlockAsync(startTime, duration))
            .ReturnsAsync(true);

        // Act
        var result = await _timeBlockService.CheckIfAvailableTimeBlockAsync(startTime, duration);

        // Assert
        Assert.IsTrue(result);
        _timeBlockRepositoryMock.Verify(repo => repo.CheckIfAvailableTimeBlockAsync(startTime, duration), Times.Once);
    }

    [Test]
    public async Task DeleteWorkingHoursAsync_ShouldCallRepository_WhenWorkingHoursAreValid()
    {
        // Arrange
        int doctorId = 1;
        var workingHoursDto = new WorkingHoursDto { StartTime = DateTime.Now.AddDays(1) };

        // Act
        await _timeBlockService.DeleteWorkingHoursAsync(doctorId, workingHoursDto);

        // Assert
        _timeBlockRepositoryMock.Verify(repo => repo.DeleteWorkingHoursAsync(doctorId, workingHoursDto), Times.Once);
    }

    [Test]
    public void DeleteWorkingHoursAsync_ShouldThrowBusinessException_WhenWorkingHoursAreInThePast()
    {
        // Arrange
        int doctorId = 1;
        var workingHoursDto = new WorkingHoursDto { StartTime = DateTime.Now.AddDays(-1) };

        // Act & Assert
        var exception = Assert.ThrowsAsync<BusinessException>(async () =>
            await _timeBlockService.DeleteWorkingHoursAsync(doctorId, workingHoursDto));
        Assert.That(exception?.Message, Is.EqualTo("Cannot delete working hours for today or past dates."));
    }

    [Test]
    public async Task AddWorkingHoursAsync_ShouldCallRepository_WhenWorkingHoursAreValid()
    {
        // Arrange
        int doctorId = 1;
        var workingHoursDto = new WorkingHoursDto { StartTime = DateTime.Now.AddDays(1) };

        // Act
        await _timeBlockService.AddWorkingHoursAsync(doctorId, workingHoursDto);

        // Assert
        _timeBlockRepositoryMock.Verify(repo => repo.AddWorkingHoursAsync(doctorId, workingHoursDto), Times.Once);
    }

    [Test]
    public void AddWorkingHoursAsync_ShouldThrowBusinessException_WhenWorkingHoursAreInThePast()
    {
        // Arrange
        int doctorId = 1;
        var workingHoursDto = new WorkingHoursDto { StartTime = DateTime.Now.AddDays(-1) };

        // Act & Assert
        var exception = Assert.ThrowsAsync<BusinessException>(async () =>
            await _timeBlockService.AddWorkingHoursAsync(doctorId, workingHoursDto));
        Assert.That(exception?.Message, Is.EqualTo("Cannot add working hours for today or past dates."));
    }
}