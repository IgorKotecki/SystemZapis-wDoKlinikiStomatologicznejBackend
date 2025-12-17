namespace SystemZapisowDoKlinikiApi.DTO;

public class ServiceUpdateDTO
{
    public decimal? LowPrice { get; set; }
    public decimal? HighPrice { get; set; }
    public int MinTime { get; set; }

    public List<int> ServiceCategoryIds { get; set; } = new();
}