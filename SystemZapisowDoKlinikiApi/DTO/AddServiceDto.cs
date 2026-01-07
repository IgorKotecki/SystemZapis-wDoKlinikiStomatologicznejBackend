namespace SystemZapisowDoKlinikiApi.DTO;

public class AddServiceDto
{
    public decimal? LowPrice { get; set; }
    public decimal? HighPrice { get; set; }
    public required int MinTime { get; set; }
    public required ICollection<LanguageDto> Languages { get; set; } = new List<LanguageDto>();
    public int? ServiceId { get; set; }
    public ICollection<int> RolePermissionIds { get; set; } = new List<int>();
    public ICollection<int> ServiceCategoriesId { get; set; } = new List<int>();
}