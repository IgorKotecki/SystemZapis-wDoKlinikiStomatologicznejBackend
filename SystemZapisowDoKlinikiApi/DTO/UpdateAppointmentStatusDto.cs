using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.Controllers;

public class UpdateAppointmentStatusDto
{
    [Required] public required string AppointmentId { get; set; }
    [Required] public required int StatusId { get; set; }
}