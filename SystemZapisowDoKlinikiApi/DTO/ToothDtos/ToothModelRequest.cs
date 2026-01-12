namespace SystemZapisowDoKlinikiApi.DTO.ToothDtos;

public class ToothModelRequest
{
    public int UserId { get; set; }
    public required string Language { get; set; }
}