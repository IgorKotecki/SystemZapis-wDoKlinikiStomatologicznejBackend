using SystemZapisowDoKlinikiApi.DTO.AdditionalInformationDtos;
using SystemZapisowDoKlinikiApi.DTO.ServiceDtos;
using SystemZapisowDoKlinikiApi.DTO.UserDtos;

namespace SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

public class AppointmentDto
{
    public required UserDto User { get; set; }
    public DateTime StartTime { get; set; }
    public required string? AppointmentGroupId { get; set; }

    public DateTime EndTime { get; set; }
    public required UserDto? Doctor { get; set; }
    public required ICollection<ServiceDto>? Services { get; set; }
    public required string Status { get; set; }
    public required ICollection<AddInformationOutDto>? AdditionalInformation { get; set; }
    public required string? Notes { get; set; }
    public required string? CancellationReason { get; set; }
}