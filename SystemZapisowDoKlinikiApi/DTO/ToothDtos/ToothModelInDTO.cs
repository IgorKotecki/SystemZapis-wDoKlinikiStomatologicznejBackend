namespace SystemZapisowDoKlinikiApi.DTO.ToothDtos;

public class ToothModelInDto
{
    public ICollection<ToothInDto> Teeth { get; set; } = new List<ToothInDto>();
    public int UserId { get; set; }

    public string AppointmentGuid { get; set; }
}