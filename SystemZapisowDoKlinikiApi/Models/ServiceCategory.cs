namespace SystemZapisowDoKlinikiApi.Models;

public class ServiceCategory
{
    public int Id { get; set; }
    public string NamePl { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public virtual ICollection<Service> Services { get; set; } = null!;
}