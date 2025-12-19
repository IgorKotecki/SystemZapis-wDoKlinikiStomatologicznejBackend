using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Services;

public class CloudinaryService : ICloudinaryRepository, ICloudinaryService
{
    private readonly ICloudinaryRepository _cloudinaryRepository;
    
    public CloudinaryService(ICloudinaryRepository cloudinaryRepository)
    {
        _cloudinaryRepository = cloudinaryRepository;
    }
    
    public CloudinarySignatureDTO GetCloudinarySignature()
    {
        return _cloudinaryRepository.GetCloudinarySignature();
    }
}