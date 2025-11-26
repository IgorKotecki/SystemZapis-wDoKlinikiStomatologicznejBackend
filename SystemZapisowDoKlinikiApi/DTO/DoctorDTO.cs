namespace SystemZapisowDoKlinikiApi.DTO;

public class DoctorDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;    
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SpecializationPl { get; set; } = string.Empty;
    public string SpecializationEn { get; set; } = string.Empty;
}