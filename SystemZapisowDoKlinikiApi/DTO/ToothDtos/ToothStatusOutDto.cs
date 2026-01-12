namespace SystemZapisowDoKlinikiApi.DTO.ToothDtos;

public class ToothStatusOutDto
{
    public int StatusId { get; set; }
    public required string StatusName { get; set; }
    public int CategoryId { get; set; }
    public required string CategoryName { get; set; }
}