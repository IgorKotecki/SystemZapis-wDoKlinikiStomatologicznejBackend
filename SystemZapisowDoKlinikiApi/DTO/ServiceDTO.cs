namespace SystemZapisowDoKlinikiApi.DTO;

public class ServiceDTO
{
    public int Id { get; set; }

    public decimal? LowPrice { get; set; }

    public decimal? HighPrice { get; set; }

    public int MinTime { get; set; }

    public string? LanguageCode { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    public ICollection<string> Catergories { get; set; }
}