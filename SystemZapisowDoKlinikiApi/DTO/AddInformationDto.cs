namespace SystemZapisowDoKlinikiApi.Controllers;

public class AddInformationDto
{
    public required string BodyPl { get; set; }
    public required string BodyEn { get; set; }
    public required string language { get; set; }
}