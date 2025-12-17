namespace SystemZapisowDoKlinikiApi.DTO;

public class BookAppointmentRequestDTO
{
    public int DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public int[] ServicesIds { get; set; }
}