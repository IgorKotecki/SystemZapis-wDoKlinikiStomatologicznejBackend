namespace SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

public class BookAppointmentRequestDto
{
    public int DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public ICollection<int> ServicesIds { get; set; } = new List<int>();
}