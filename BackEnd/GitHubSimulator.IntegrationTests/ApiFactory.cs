using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;

namespace GitHubSimulator.IntegrationTests;

public class ApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    // Creates a disposable database container for every integration test
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
            .WithUsername("user")
            .WithPassword("password")
            .Build();

    public HttpClient HttpClient = default!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Used to store the configuration part that has the changed connection string
        IConfigurationSection? databaseConfigurationSection = null;
        
        builder.ConfigureAppConfiguration((context, config) => 
        {
           // Load the app settings for tests
           config.AddJsonFile("appsettings.json");
           // Replace the connection string in the configuration with the connection string from the container
           var mongoDbConfigurationSection = config.Build().GetSection(DatabaseSettings.SectionName);
           mongoDbConfigurationSection["ConnectionString"] = _mongoDbContainer.GetConnectionString();
           databaseConfigurationSection = mongoDbConfigurationSection;
        });
        
        builder.ConfigureTestServices(services =>
        {
            // Replace the configuration with the old database connection string with the new one 
            if (databaseConfigurationSection != null)
            {
                services.Configure<DatabaseSettings>(databaseConfigurationSection);
            }
        });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        HttpClient = CreateClient();
    }

    public new async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }
}