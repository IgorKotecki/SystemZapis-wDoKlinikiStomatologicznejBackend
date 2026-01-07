namespace SystemZapisowDoKlinikiApi.Models;

public class ToothStatusCategoryTranslation
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public required string LanguageCode { get; set; }
    public required string Name { get; set; }

    public required ToothStatusCategory Category { get; set; }
}