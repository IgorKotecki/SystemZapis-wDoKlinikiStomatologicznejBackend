namespace SystemZapisowDoKlinikiApi.DTO.UserDtos;

public class TeamDto
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Surname { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public string? RoleName { get; set; }
    public string? SpecializationPl { get; set; }
    public string? SpecializationEn { get; set; }
    public string? PhotoURL { get; set; } = null!;
}