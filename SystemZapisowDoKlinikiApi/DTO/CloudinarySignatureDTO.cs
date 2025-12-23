namespace SystemZapisowDoKlinikiApi.DTO;

public class CloudinarySignatureDTO
{
    public string Signature { get; set; } = null!;
    public long Timestamp { get; set; }
    public string CloudName { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}