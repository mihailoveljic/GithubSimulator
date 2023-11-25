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
            .WithPassword("password")
            .Build();

    public HttpClient HttpClient = default!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) => 
        {
           // Load the app settings for tests
           config.AddJsonFile("appsettings.json");
        });

        builder.ConfigureTestServices(services =>
        {
            // Replace the DatabaseSettings with the test-specific settings
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var databaseSettings = configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>();
            databaseSettings.ConnectionString = _mongoDbContainer.GetConnectionString();

            services.AddSingleton(databaseSettings);
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