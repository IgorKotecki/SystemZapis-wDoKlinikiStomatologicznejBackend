namespace SystemZapisowDoKlinikiApi.DTO.UserDtos;

public class UserDto
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;

    public string? Surname { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public string? PhoneNumber { get; set; } = null!;

    public string? RoleName { get; set; } = null!;
    public string? PhotoURL { get; set; } = null!;
}