namespace SystemZapisowDoKlinikiApi.DTO;

public class BookAppointmentRequestDTO
{
    public int DoctorId {get; set;}
    public int TimeBlockId {get; set;}
    public ICollection<int> ServiceIds {get; set;}
}