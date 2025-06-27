using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class ServicesTranslation
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
