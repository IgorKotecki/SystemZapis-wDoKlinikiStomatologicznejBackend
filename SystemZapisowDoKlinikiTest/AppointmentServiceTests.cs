using System.Globalization;
using Moq;
using SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;
using SystemZapisowDoKlinikiApi.Exceptions;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Security;
using SystemZapisowDoKlinikiApi.Services;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiTest;

[TestFixture]
public class AppointmentServiceTests
{
    private Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private Mock<IUserService> _userServiceMock;
    private Mock<IEmailService> _emailServiceMock;
    private AppointmentService _appointmentService;

    [SetUp]
    public void SetUp()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _userServiceMock = new Mock<IUserService>();
        _emailServiceMock = new Mock<IEmailService>();
        _appointmentService = new AppointmentService(
            _appointmentRepositoryMock.Object,
            _userServiceMock.Object,
            _emailServiceMock.Object
        );
    }

    [Test]
    public void ValidateUserId_ShouldThrowArgumentException_WhenUserIdIsInvalid()
    {
        // Arrange
        int invalidUserId = -1;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            AppointmentService.ValidateUserId(invalidUserId));
        Assert.That(exception?.Message, Is.EqualTo("User ID must be a positive integer. (Parameter 'userId')"));
    }

    [Test]
    public void EnsureAppointmentIsNotInPast_ShouldThrowBusinessException_WhenDateIsInThePast()
    {
        // Arrange
        DateTime pastDate = DateTime.Now.AddDays(-1);

        // Act & Assert
        var exception = Assert.Throws<BusinessException>(() =>
            AppointmentService.EnsureAppointmentIsNotInPast(pastDate));
        Assert.That(exception?.Message, Is.EqualTo("Cannot book an appointment in the past."));
    }

    [Test]
    public async Task GetAppointmentsByUserIdAsync_ShouldReturnAppointments_WhenCalled()
    {
        // Arrange
        int userId = 1;
        string lang = "en";
        bool showCancelled = false;
        bool showCompleted = true;
        bool showPlanned = true;

        var expectedAppointments = new List<AppointmentDto>
        {
            new AppointmentDto
            {
                StartTime = DateTime.Now.AddHours(1),
                User = null!,
                AppointmentGroupId = null,
                Doctor = null,
                Services = null,
                Status = null!,
                AdditionalInformation = null,
                Notes = null,
                CancellationReason = null
            }
        };

        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentsByUserIdAsync(userId, lang, showCancelled, showCompleted, showPlanned))
            .ReturnsAsync(expectedAppointments);

        // Act
        var result =
            await _appointmentService.GetAppointmentsByUserIdAsync(userId, lang, showCancelled, showCompleted,
                showPlanned);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result, Is.EqualTo(expectedAppointments));
    }

    [Test]
    public Task CancelAppointmentAsync_ShouldThrowBusinessException_WhenAppointmentIsInThePast()
    {
        // Arrange
        var cancellationDto = new CancellationDto
        {
            AppointmentGuid = "test-guid",
            Reason = null!
        };

        var pastAppointment = new AppointmentDto
        {
            StartTime = DateTime.Now.AddDays(-1),
            User = null!,
            AppointmentGroupId = null,
            Doctor = null,
            Services = null,
            Status = null!,
            AdditionalInformation = null,
            Notes = null,
            CancellationReason = null
        };

        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentsByGuidAsync(cancellationDto.AppointmentGuid))
            .ReturnsAsync(pastAppointment);

        // Act & Assert
        var exception = Assert.ThrowsAsync<BusinessException>(async () =>
            await _appointmentService.CancelAppointmentAsync(cancellationDto));
        Assert.That(exception?.Message, Is.EqualTo("Cannot cancel an appointment that is in the past."));
        return Task.CompletedTask;
    }

    [Test]
    public async Task SendConfirmationEmailAsync_ShouldCallEmailService_WhenCalled()
    {
        // Arrange
        var user = new User { Email = "test@example.com", Name = "John" };
        DateTime startTime = DateTime.Now.AddHours(1);

        // Act
        await _appointmentService.SendConfirmationEmailAsync(user, startTime);

        // Assert
        _emailServiceMock.Verify(emailService =>
            emailService.SendEmailAsync(
                user.Email,
                "Appointment Confirmation",
                It.Is<string>(body =>
                    body.Contains("John") && body.Contains(startTime.ToString(CultureInfo.InvariantCulture)))
            ), Times.Once);
    }
}