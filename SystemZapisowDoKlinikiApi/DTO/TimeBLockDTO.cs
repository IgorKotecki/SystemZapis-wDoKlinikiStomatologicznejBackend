namespace SystemZapisowDoKlinikiApi.DTO;

public class TimeBlockDto
{
    public UserDTO User { get; set; }

    public int DoctorBlockId { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }
    public Boolean isAvailable { get; set; }
}