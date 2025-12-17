using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Controllers;

public class BookAppointmentRequestWithUserIdDto
{
    public required int UserId { get; set; }
    public required BookAppointmentRequestDTO BookAppointmentRequestDto { get; set; }
}