using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.AuthDtos;

public class RegisterRequest
{
    [Required(ErrorMessage = "Imię jest wymagane")]
    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "Imię zawiera niedozwolone znaki")]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "Nazwisko zawiera niedozwolone znaki")]
    [StringLength(50, MinimumLength = 2)]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Numer telefonu jest wymagany")]
    [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Hasło musi mieć minimum 8 znaków")]
    public string Password { get; set; } = null!;
}