namespace SystemZapisowDoKlinikiApi.Models;

public partial class Service
{
    public int Id { get; set; }

    public decimal? LowPrice { get; set; } = null!;

    public decimal? HighPrice { get; set; }

    public int MinTime { get; set; }
    
    public string? PhotoUrl { get; set; }

    public virtual ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();

    public virtual ICollection<ServiceDependency> ServiceDependencyRequiredServices { get; set; } =
        new List<ServiceDependency>();

    public virtual ICollection<ServiceDependency> ServiceDependencyServices { get; set; } =
        new List<ServiceDependency>();

    public virtual ICollection<ServicesTranslation> ServicesTranslations { get; set; } =
        new List<ServicesTranslation>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}