using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class TimeBlock
{
    public int Id { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public virtual ICollection<DoctorBlock> DoctorBlocks { get; set; } = new List<DoctorBlock>();
}
