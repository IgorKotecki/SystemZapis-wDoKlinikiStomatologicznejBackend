namespace SystemZapisowDoKlinikiApi.DTO;

public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? RoleName { get; set; } = null!;
}