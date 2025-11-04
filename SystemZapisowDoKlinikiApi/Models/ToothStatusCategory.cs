namespace SystemZapisowDoKlinikiApi.Models;

public class ToothStatusCategory
{
    public int Id { get; set; }
    
    public ICollection<ToothStatusCategoryTranslation> Translations { get; set; }
    public ICollection<ToothStatus> ToothStatuses { get; set; }
}