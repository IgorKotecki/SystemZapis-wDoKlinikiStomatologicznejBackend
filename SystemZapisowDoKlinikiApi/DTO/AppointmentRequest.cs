namespace SystemZapisowDoKlinikiApi.DTO;

public class AppointmentRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public ICollection<int> DoctorBlockId { get; set; }
    public ServiceRequest Service { get; set; }
}