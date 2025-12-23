namespace SystemZapisowDoKlinikiApi.DTO;

public class AddServiceDto
{
    public decimal? LowPrice { get; set; }
    public decimal? HighPrice { get; set; }
    public required int MinTime { get; set; }
    public required ICollection<LanguageDto> Languages { get; set; }
    public int? ServiceId { get; set; }
    public ICollection<int> rolePermissionIds { get; set; }
    public ICollection<int> ServiceCategoriesId { get; set; }
}