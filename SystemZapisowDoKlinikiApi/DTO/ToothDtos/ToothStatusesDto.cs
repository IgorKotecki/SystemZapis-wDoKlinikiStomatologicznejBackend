namespace SystemZapisowDoKlinikiApi.DTO.ToothDtos;

public class ToothStatusesDto
{
    public Dictionary<string, List<ToothStatusOutDto>> StatusesByCategories { get; set; } =
        new Dictionary<string, List<ToothStatusOutDto>>();
}