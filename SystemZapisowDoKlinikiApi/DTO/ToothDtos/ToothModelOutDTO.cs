namespace SystemZapisowDoKlinikiApi.DTO.ToothDtos;

public class ToothModelOutDto
{
    public ICollection<ToothOutDto> Teeth { get; set; } = new List<ToothOutDto>();
}