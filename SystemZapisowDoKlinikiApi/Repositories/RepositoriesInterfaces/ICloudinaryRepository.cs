using SystemZapisowDoKlinikiApi.DTO;

namespace SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

public interface ICloudinaryRepository
{
    public CloudinarySignatureDTO GetCloudinarySignature();
}