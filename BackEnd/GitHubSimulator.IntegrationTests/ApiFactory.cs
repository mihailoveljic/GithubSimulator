using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GitHubSimulator.IntegrationTests;

public class ApiFactory : WebApplicationFactory<IApiMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // builder.ConfigureServices(services =>
        // {
        //     var descriptor = services.SingleOrDefault(
        //         d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
        //
        //     if (descriptor != null)
        //     {
        //         services.Remove(descriptor);
        //     }
        //
        //     var connectionString = string.Empty;
        //     
        //     services.AddDbContext<ApplicationDbContext>(opts => opts.)
        // })
    }
}