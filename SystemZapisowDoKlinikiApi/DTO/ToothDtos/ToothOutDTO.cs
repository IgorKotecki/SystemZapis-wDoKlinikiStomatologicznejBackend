namespace SystemZapisowDoKlinikiApi.DTO.ToothDtos;

public class ToothOutDto
{
    public required ToothStatusOutDto Status { get; set; }
    public int ToothNumber { get; set; }
    public required string ToothName { get; set; }
}