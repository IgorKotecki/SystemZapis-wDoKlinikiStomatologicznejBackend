using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO;

public class AppointmentRequest
{
    [Required] public string Name { get; set; }
    [Required] public string Surname { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] [Phone] [MinLength(9)] public string PhoneNumber { get; set; }
    [Required] public int DoctorId { get; set; }
    [Required] public DateTime StartTime { get; set; }
    [Required] public int Duration { get; set; }
    [Required] public int[] ServicesIds { get; set; }
}