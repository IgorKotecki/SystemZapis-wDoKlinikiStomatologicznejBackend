using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.UserDtos;

public class UserUpdateDto
{
    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "NAME_INVALID_CHARACTERS")]
    [StringLength(50, ErrorMessage = "NAME_LENGTH_INVALID")]
    public string? Name { get; set; }

    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "SURNAME_INVALID_CHARACTERS")]
    [StringLength(50, ErrorMessage = "SURNAME_LENGTH_INVALID")]
    public string? Surname { get; set; }

    [Phone(ErrorMessage = "PHONE_NUMBER_INVALID_FORMAT")]
    public string? PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "EMAIL_INVALID_FORMAT")]
    public string? Email { get; set; }

    [Url] public string? PhotoUrl { get; set; }
}