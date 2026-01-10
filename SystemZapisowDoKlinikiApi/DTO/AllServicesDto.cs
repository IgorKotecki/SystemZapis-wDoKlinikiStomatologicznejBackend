namespace SystemZapisowDoKlinikiApi.DTO;

public class AllServicesDto
{
    public Dictionary<string, ICollection<ServiceDTO>> ServicesByCategory { get; set; } =
        new Dictionary<string, ICollection<ServiceDTO>>();
}