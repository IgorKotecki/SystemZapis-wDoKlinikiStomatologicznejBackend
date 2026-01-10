namespace SystemZapisowDoKlinikiApi.DTO;

public class ToothModelInDTO
{
    public ICollection<ToothInDTO> Teeth { get; set; } = new List<ToothInDTO>();
    public int UserId { get; set; }
}