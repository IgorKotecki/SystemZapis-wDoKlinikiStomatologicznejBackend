using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class Doctor
{
    public int UserId { get; set; }

    public string SpecializationPl { get; set; } = null!;
    public string SpecializationEn { get; set; } = null!;

    public string? ImgPath { get; set; }

    public virtual ICollection<DaySchemeTimeBlock> DaySchemeTimeBlocks { get; set; } = new List<DaySchemeTimeBlock>();

    public virtual ICollection<DoctorBlock> DoctorBlocks { get; set; } = new List<DoctorBlock>();

    public virtual User User { get; set; } = null!;
}
