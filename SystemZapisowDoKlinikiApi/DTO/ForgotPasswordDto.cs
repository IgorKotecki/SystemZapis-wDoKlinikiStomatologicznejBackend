using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO;

public class ForgotPasswordDto
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}