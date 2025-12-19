using Microsoft.AspNetCore.Mvc;
using SystemZapisowDoKlinikiApi.Services.ServiceInterfaces;

namespace SystemZapisowDoKlinikiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CloudinaryController: ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;
    
    public CloudinaryController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }
    
    [HttpGet("signature")]
    public IActionResult GetSignature()
    {
        return Ok(_cloudinaryService.GetCloudinarySignature());
    }
}