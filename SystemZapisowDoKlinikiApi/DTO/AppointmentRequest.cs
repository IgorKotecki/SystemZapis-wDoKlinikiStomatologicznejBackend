namespace SystemZapisowDoKlinikiApi.DTO;

public class AppointmentRequest
{
    public int UserId { get; set; }

    public int DoctorBlockId { get; set; }
    public ServiceRequest Service { get; set; }
}