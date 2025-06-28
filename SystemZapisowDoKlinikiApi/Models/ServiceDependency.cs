using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class ServiceDependency
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int RequiredServiceId { get; set; }

    public virtual Service RequiredService { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
