using Microsoft.Extensions.Logging.Abstractions;

namespace SystemZapisowDoKlinikiApi.DTO;

public class AddServiceDto
{
    public decimal LowPrice { get; set; }
    public decimal HighPrice { get; set; }
    public int MinTime { get; set; }
    public ICollection<LanguageDto> Languages { get; set; }
    public int? serviceId { get; set; } 
    public ICollection<int> rolePermissionIds { get; set; }
}