namespace SystemZapisowDoKlinikiApi.Models;

public class ToothStatusCategoryTranslation
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string LanguageCode { get; set; }
    public string Name { get; set; }

    public ToothStatusCategory Category { get; set; }
}