namespace SystemZapisowDoKlinikiApi.DTO;

public class CompletionDto
{
    public required string AppointmentGroupId { get; set; }
    public string? Notes { get; set; }
}