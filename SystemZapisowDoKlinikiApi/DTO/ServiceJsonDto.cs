namespace SystemZapisowDoKlinikiApi.DTO;

public class ServiceJsonDto
{
    public int Id { get; set; }
    public decimal? LowPrice { get; set; }
    public decimal? HighPrice { get; set; }
    public int MinTime { get; set; }
    public List<ServiceTranslationDto> Translations { get; set; }
    public List<ServiceCategoryDto> Categories { get; set; }
}

public class ServiceTranslationDto
{
    public string LanguageCode { get; set; }
    public string Name { get; set; }
}

public class ServiceCategoryDto
{
    public int Id { get; set; }
    public string NamePl { get; set; }
    public string NameEn { get; set; }
}