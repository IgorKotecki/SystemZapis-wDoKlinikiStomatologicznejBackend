namespace SystemZapisowDoKlinikiApi.DTO;

public class ToothOutDTO
{
    public required ToothStatusOutDto Status { get; set; }
    public int ToothNumber { get; set; }
    public required string ToothName { get; set; }
}