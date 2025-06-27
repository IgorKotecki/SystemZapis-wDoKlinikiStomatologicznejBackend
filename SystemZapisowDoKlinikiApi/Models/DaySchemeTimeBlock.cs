using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class DaySchemeTimeBlock
{
    public int Id { get; set; }

    public int DoctorUserId { get; set; }

    public int WeekDay { get; set; }

    public TimeOnly JobStart { get; set; }

    public TimeOnly JobEnd { get; set; }

    public virtual Doctor DoctorUser { get; set; } = null!;
}
