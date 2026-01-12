using SystemZapisowDoKlinikiApi.DTO.UserDtos;

namespace SystemZapisowDoKlinikiApi.DTO.TimeBlocksDtos;

public class TimeBlockDto
{
    public required UserDto User { get; set; }

    public int DoctorBlockId { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }
    public Boolean IsAvailable { get; set; }
}