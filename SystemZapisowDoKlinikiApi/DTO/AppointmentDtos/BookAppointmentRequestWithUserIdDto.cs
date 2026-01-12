namespace SystemZapisowDoKlinikiApi.DTO.AppointmentDtos;

public class BookAppointmentRequestWithUserIdDto
{
    public required int UserId { get; set; }
    public required BookAppointmentRequestDto BookAppointmentRequestDto { get; set; }
}