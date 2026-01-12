using System.ComponentModel.DataAnnotations;

namespace SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

public class UpdateAppointmentStatusDto
{
    [Required] public required string AppointmentId { get; set; }
    [Required] public required int StatusId { get; set; }
}