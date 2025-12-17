namespace SystemZapisowDoKlinikiApi.Models;

public class ToothName
{
    public int Id { get; set; }
    public int ToothNumber { get; set; }
    public string NamePl { get; set; } = null!;
    public string NameEn { get; set; } = null!;
}