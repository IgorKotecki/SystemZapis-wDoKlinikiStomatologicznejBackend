namespace SystemZapisowDoKlinikiApi.DTO;

public class CancellationDto
{
    public required string AppointmentGuid { get; set; }
    public required string Reason { get; set; }
}