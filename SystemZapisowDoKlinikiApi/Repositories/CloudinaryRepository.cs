using System.Security.Cryptography;
using System.Text;
using SystemZapisowDoKlinikiApi.DTO;
using SystemZapisowDoKlinikiApi.Models;
using SystemZapisowDoKlinikiApi.Repositories.RepositoriesInterfaces;

namespace SystemZapisowDoKlinikiApi.Repositories;

public class CloudinaryRepository : ICloudinaryRepository
{
    private readonly IConfiguration _configuration;

    public CloudinaryRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public CloudinarySignatureDTO GetCloudinarySignature()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var apiSecret = _configuration["Cloudinary:ApiSecret"];

        var toSign = $"timestamp={timestamp}";

        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(
            Encoding.UTF8.GetBytes(toSign + apiSecret)
        );

        var signature = BitConverter
            .ToString(hash)
            .Replace("-", "")
            .ToLower();

        return new CloudinarySignatureDTO
        {
            Signature = signature,
            Timestamp = timestamp,
            CloudName = _configuration["Cloudinary:CloudName"],
            ApiKey = _configuration["Cloudinary:ApiKey"]
        };
    }
}