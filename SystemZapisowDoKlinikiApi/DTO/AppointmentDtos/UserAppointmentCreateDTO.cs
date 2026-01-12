namespace SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

public class UserAppointmentCreateDto
{
    public int DoctorId { get; set; }
    public int TimeBlockId { get; set; }
    public List<int> ServiceIds { get; set; } = new List<int>();
}