namespace SystemZapisowDoKlinikiApi.DTO.UserDtos;

public class UserUpdateDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? PhotoUrl { get; set; }
}