using System;
using System.Collections.Generic;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int RolesId { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpDate { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Doctor? Doctor { get; set; }

    public virtual Role Roles { get; set; } = null!;

    public virtual ICollection<Tooth> Teeth { get; set; } = new List<Tooth>();
}
