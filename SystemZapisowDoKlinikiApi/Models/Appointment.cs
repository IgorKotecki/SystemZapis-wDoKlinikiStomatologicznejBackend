namespace SystemZapisowDoKlinikiApi.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DoctorBlockId { get; set; }

    public string? AppointmentGroupId { get; set; }
    public int AppointmentStatusId { get; set; }

    public string? CancellationReason { get; set; }

    public virtual DoctorBlock DoctorBlock { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<AdditionalInformation> AdditionalInformations { get; set; } =
        new List<AdditionalInformation>();

    public virtual AppointmentStatus AppointmentStatus { get; set; } = null!;
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}