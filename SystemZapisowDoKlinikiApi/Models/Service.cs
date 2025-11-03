using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class Service
{
    public int Id { get; set; }

    public decimal LowPrice { get; set; }

    public decimal HighPrice { get; set; }

    public int MinTime { get; set; }

    public string Category { get; set; }

    public virtual ICollection<ServiceDependency> ServiceDependencyRequiredServices { get; set; } = new List<ServiceDependency>();

    public virtual ICollection<ServiceDependency> ServiceDependencyServices { get; set; } = new List<ServiceDependency>();

    public virtual ICollection<ServicesTranslation> ServicesTranslations { get; set; } = new List<ServicesTranslation>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
