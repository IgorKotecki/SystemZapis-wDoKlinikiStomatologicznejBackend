namespace SystemZapisowDoKlinikiApi.DTO;

public class ToothModelOutDTO
{
    public ICollection<ToothOutDTO> Teeth { get; set; } = new List<ToothOutDTO>();
}