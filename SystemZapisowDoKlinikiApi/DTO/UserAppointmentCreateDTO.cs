namespace SystemZapisowDoKlinikiApi.DTO;

public class UserAppointmentCreateDTO
{
    public int DoctorId { get; set; }
    public int TimeBlockId { get; set; }
    public List<int> ServiceIds { get; set; }
}