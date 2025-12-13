namespace SystemZapisowDoKlinikiApi.Controllers;

public class AddInfoToAppointmentDto
{
    public required string Id { get; set; }
    public required ICollection<int> AddInformationIds { get; set; }
}