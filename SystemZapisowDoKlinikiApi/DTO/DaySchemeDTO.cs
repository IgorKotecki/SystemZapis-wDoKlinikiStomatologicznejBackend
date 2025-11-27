namespace SystemZapisowDoKlinikiApi.DTO;

public class DaySchemeDto
{
    public int DayOfWeek { get; set; }
    public TimeOnly StartHour { get; set; }
    public TimeOnly EndHour { get; set; }
}