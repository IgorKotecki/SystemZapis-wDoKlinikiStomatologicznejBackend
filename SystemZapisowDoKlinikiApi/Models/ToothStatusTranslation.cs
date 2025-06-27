using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class ToothStatusTranslation
{
    public int Id { get; set; }

    public int ToothStatusId { get; set; }

    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ToothStatus ToothStatus { get; set; } = null!;
}
