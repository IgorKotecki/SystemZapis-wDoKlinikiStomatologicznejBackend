namespace SystemZapisowDoKlinikiApi.DTO.ServiceDtos;

public class AllServicesDto
{
    public Dictionary<string, ICollection<ServiceDto>> ServicesByCategory { get; set; } =
        new Dictionary<string, ICollection<ServiceDto>>();
}