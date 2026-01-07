namespace SystemZapisowDoKlinikiApi.DTO;

public class BookAppointmentRequestDTO
{
    public int DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public ICollection<int> ServicesIds { get; set; } = new List<int>();
}