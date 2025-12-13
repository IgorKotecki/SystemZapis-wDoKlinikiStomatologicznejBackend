namespace SystemZapisowDoKlinikiApi.DTO;

public class ToothStatusesDto
{
    public Dictionary<string, List<ToothStatusOutDto>> StatusesByCategories { get; set; }
}