using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

public class AppointmentRequest
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    [EmailAddress] public required string Email { get; set; }
    [Phone] [MinLength(9)] public required string PhoneNumber { get; set; }
    public int DoctorId { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public ICollection<int> ServicesIds { get; set; } = new List<int>();
}