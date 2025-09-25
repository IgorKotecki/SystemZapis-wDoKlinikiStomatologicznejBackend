namespace SystemZapisowDoKlinikiApi.DTO;

public class AddDoctorDto
{
    public int UserId { get; set; }
    public string SpecializationPl { get; set; }
    public string SpecializationEn { get; set; }
    public string ImageUrl { get; set; }
}