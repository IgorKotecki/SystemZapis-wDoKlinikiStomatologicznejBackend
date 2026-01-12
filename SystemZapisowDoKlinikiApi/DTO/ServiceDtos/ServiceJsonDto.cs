namespace SystemZapisowDoKlinikiApi.DTO.ServiceDtos;

public class ServiceJsonDto
{
    public int Id { get; set; }
    public decimal? LowPrice { get; set; }
    public decimal? HighPrice { get; set; }
    public int MinTime { get; set; }
    public List<ServiceTranslationDto> Translations { get; set; }
    public List<ServiceCategoryJsonDto> Categories { get; set; }
}

public class ServiceTranslationDto
{
    public string LanguageCode { get; set; }
    public string Name { get; set; }
}

public class ServiceCategoryJsonDto
{
    public int Id { get; set; }
    public string NamePl { get; set; }
    public string NameEn { get; set; }
}