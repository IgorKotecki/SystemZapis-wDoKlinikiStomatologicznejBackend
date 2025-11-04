namespace SystemZapisowDoKlinikiApi.DTO;

public class ToothModelInDTO
{
    public ICollection<ToothInDTO> Teeth { get; set; }
    public int UserId { get; set; }
}