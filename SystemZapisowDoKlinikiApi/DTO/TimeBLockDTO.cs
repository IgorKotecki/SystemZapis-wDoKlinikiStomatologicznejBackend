namespace SystemZapisowDoKlinikiApi.DTO;

public class TimeBlockDto
{
    public required UserDTO User { get; set; }

    public int DoctorBlockId { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }
    public Boolean IsAvailable { get; set; }
}