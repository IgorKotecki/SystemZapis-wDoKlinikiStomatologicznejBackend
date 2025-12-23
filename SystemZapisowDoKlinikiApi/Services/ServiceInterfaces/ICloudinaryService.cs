using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

public interface ICloudinaryService
{
    public CloudinarySignatureDTO GetCloudinarySignature();
}