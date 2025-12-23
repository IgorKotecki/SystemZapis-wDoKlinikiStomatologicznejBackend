namespace SystemZapisowDoKlinikiApi.DTO;

public class AppointmentDto
{
    public required UserDTO User { get; set; }
    public DateTime StartTime { get; set; }
    public required string AppointmentGroupId { get; set; }

    public DateTime EndTime { get; set; }
    public required UserDTO Doctor { get; set; }
    public required ICollection<ServiceDTO> Services { get; set; }
    public required string Status { get; set; }
}