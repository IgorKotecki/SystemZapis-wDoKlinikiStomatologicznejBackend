namespace SystemZapisowDoKlinikiApi.DTO;

public class AppointmentDto
{
    public UserDTO User { get; set; }

    public DateTime StartTime { get; set; }

    public string AppointmentGroupId { get; set; }

    public DateTime EndTime { get; set; }
    public UserDTO Doctor { get; set; }
    public ICollection<ServiceDTO> Services { get; set; }
    public string Status { get; set; }
}