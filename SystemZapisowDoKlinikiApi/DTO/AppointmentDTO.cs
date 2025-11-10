namespace SystemZapisowDoKlinikiApi.DTO;

public class AppointmentDto
{
    public int Id { get; set; }

    public UserDTO User { get; set; }

    public TimeBlockDto DoctorBlock { get; set; }
    public ICollection<ServiceDTO> Services { get; set; }
    public string Status { get; set; }
}