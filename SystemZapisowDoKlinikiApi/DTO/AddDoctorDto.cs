namespace SystemZapisowDoKlinikiApi.DTO;

public class AddDoctorDto
{
    public required int UserId { get; set; }
    public required string SpecializationPl { get; set; }
    public required string SpecializationEn { get; set; }
    public string? ImageUrl { get; set; }
}