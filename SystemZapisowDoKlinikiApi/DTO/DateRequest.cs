namespace SystemZapisowDoKlinikiApi.DTO;

public class DateRequest
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }

    public override string ToString()
    {
        return $"{Year:D4}-{Month:D2}-{Day:D2}";
    }
}