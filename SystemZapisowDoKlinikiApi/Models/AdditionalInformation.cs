namespace SystemZapisowDoKlinikiApi.Models;

public partial class AdditionalInformation
{
    public int Id { get; set; }

    public string BodyPl { get; set; } = null!;

    public string BodyEn { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}