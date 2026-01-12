namespace SystemZapisowDoKlinikiApi.DTO.DaySchemeDtos;

public class DaySchemeDto
{
    public int DayOfWeek { get; set; }
    public TimeOnly StartHour { get; set; }
    public TimeOnly EndHour { get; set; }
}