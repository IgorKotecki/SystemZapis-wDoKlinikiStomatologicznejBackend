using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class ToothStatus
{
    public int Id { get; set; }

    public virtual ICollection<Tooth> Teeth { get; set; } = new List<Tooth>();

    public virtual ICollection<ToothStatusTranslation> ToothStatusTranslations { get; set; } = new List<ToothStatusTranslation>();
}
