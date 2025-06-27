using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class DoctorBlock
{
    public int Id { get; set; }

    public int TimeBlockId { get; set; }

    public int DoctorUserId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Doctor DoctorUser { get; set; } = null!;

    public virtual TimeBlock TimeBlock { get; set; } = null!;
}
