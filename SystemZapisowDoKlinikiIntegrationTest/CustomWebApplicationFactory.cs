using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SystemZapisowDoKlinikiIntegrationTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
    }
}