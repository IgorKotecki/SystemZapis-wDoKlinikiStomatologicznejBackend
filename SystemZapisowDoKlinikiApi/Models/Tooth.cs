using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class Tooth
{
    public int Id { get; set; }

    public int ToothStatusId { get; set; }

    public int ToothNumber { get; set; }

    public int UserId { get; set; }

    public virtual ToothStatus ToothStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
