namespace SystemZapisowDoKlinikiApi.Models;

public class CompletedAppointment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? AppointmentGroupId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int DoctorId { get; set; }

    public string? ServicesJson { get; set; }

    public int AppointmentStatusId { get; set; }

    public virtual User User { get; set; } = null!;

    public string? AdditionalInformationJson { get; set; }

    public string? Notes { get; set; }

    public virtual AppointmentStatus AppointmentStatus { get; set; } = null!;
}