using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.AuthDtos;

public class ForgotPasswordDto
{
    [EmailAddress] public string Email { get; set; } = string.Empty;
}