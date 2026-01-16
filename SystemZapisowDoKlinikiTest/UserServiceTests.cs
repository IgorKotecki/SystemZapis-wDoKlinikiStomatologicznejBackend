using Moq;
using SystemZapisowDoKlinikiApi.DTO.UserDtos;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services;

namespace SystemZapisowDoKlinikiTest;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Test]
    public async Task CreateGuestUserAsync_ShouldCallRepository_WhenDetailsAreValid()
    {
        // Arrange
        string name = "John", surname = "Doe", email = "john.doe@example.com", phoneNumber = "123456789";
        var expectedUser = new User();

        _userRepositoryMock
            .Setup(repo => repo.CreateGuestUserAsync(name, surname, email, phoneNumber))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.CreateGuestUserAsync(name, surname, email, phoneNumber);

        // Assert
        Assert.That(result, Is.EqualTo(expectedUser));
        _userRepositoryMock.Verify(repo => repo.CreateGuestUserAsync(name, surname, email, phoneNumber), Times.Once);
    }

    [Test]
    public void CreateGuestUserAsync_ShouldThrowArgumentException_WhenDetailsAreInvalid()
    {
        // Arrange
        string name = "", surname = "Doe", email = "john.doe@example.com", phoneNumber = "123456789";

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.CreateGuestUserAsync(name, surname, email, phoneNumber));
        Assert.That(exception?.Message, Is.EqualTo("All user details must be provided."));
    }

    [Test]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenEmailIsValid()
    {
        // Arrange
        string email = "john.doe@example.com";
        var expectedUser = new User();

        _userRepositoryMock
            .Setup(repo => repo.GetUserByEmailAsync(email))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserByEmailAsync(email);

        // Assert
        Assert.That(result, Is.EqualTo(expectedUser));
        _userRepositoryMock.Verify(repo => repo.GetUserByEmailAsync(email), Times.Once);
    }

    [Test]
    public void GetUserByEmailAsync_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        // Arrange
        string email = "";

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _userService.GetUserByEmailAsync(email));
        Assert.That(exception?.Message, Is.EqualTo("Email cannot be null or empty. (Parameter 'email')"));
    }

    [Test]
    public async Task UpdateUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        int userId = 1;
        var dto = new UserUpdateDto();

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.UpdateUserAsync(userId, dto);

        // Assert
        Assert.IsNull(result);
        _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Never);
    }
}