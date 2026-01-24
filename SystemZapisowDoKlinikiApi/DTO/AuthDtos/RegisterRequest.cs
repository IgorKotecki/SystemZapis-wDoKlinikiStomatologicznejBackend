using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.AuthDtos;

public class RegisterRequest
{
    [Required(ErrorMessage = "NAME_REQUIRED")]
    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "NAME_INVALID_CHARACTERS")]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "SURNAME_REQUIRED")]
    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "SURNAME_INVALID_CHARACTERS")]
    [StringLength(50, MinimumLength = 2)]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "EMAIL_REQUIRED")]
    [EmailAddress(ErrorMessage = "EMAIL_INVALID_FORMAT")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "PHONE_NUMBER_REQUIRED")]
    [Phone(ErrorMessage = "PHONE_NUMBER_INVALID_FORMAT")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "PASSWORD_REQUIRED")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "PASSWORD_LENGTH_INVALID")]
    public string Password { get; set; } = null!;
}