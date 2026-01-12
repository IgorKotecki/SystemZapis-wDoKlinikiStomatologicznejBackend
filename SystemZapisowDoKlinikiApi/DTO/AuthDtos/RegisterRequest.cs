using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.AuthDtos;

public class RegisterRequest
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    [EmailAddress] public string Email { get; set; } = null!;
    [Phone] public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;
}