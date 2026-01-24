using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.UserDtos;

public class UserUpdateDto
{
    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "Imię zawiera niedozwolone znaki")]
    [StringLength(50)]
    public string? Name { get; set; }

    [RegularExpression(@"^[\p{L}\s-]+$", ErrorMessage = "Nazwisko zawiera niedozwolone znaki")]
    [StringLength(50)]
    public string? Surname { get; set; }

    [Phone] public string? PhoneNumber { get; set; }

    [EmailAddress] public string? Email { get; set; }

    [Url] public string? PhotoUrl { get; set; }
}