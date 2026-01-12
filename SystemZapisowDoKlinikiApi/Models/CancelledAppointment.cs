namespace SystemZapisowDoKlinikiApi.Models;

public class CancelledAppointment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? AppointmentGroupId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? CancellationReason { get; set; }

    public int DoctorId { get; set; }

    public string? ServicesJson { get; set; }

    public int AppointmentStatusId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual AppointmentStatus AppointmentStatus { get; set; } = null!;
}