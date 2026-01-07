namespace SystemZapisowDoKlinikiApi.Models;

public class ToothStatusCategory
{
    public int Id { get; set; }

    public ICollection<ToothStatusCategoryTranslation> Translations { get; set; } =
        new List<ToothStatusCategoryTranslation>();

    public ICollection<ToothStatus> ToothStatuses { get; set; } = new List<ToothStatus>();
}