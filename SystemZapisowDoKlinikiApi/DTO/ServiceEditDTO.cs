namespace SystemZapisowDoKlinikiApi.DTO;

public class ServiceEditDTO
{
    public int Id { get; set; }
    public decimal? LowPrice { get; set; }
    public decimal? HighPrice { get; set; }
    public int MinTime { get; set; }
    public string NamePl { get; set; } = string.Empty;
    public string DescriptionPl { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public List<int> ServiceCategoryIds { get; set; } = new();
}