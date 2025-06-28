namespace SystemZapisowDoKlinikiApi.DTO;

public class AppointmentDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DoctorBlockId { get; set; }
    public ICollection<ServiceDTO> Services { get; set; }
}