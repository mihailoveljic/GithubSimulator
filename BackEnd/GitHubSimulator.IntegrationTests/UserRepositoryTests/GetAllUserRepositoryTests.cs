using System.Net;
using FluentAssertions;

namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;

public class GetAllUserRepositoryTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    
    public GetAllUserRepositoryTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }

    [Fact]
    public async Task GetAllUserRepository_ReturnsErrorWhenUserNotFound()
    {
        // Arrange
        
        // Act
        var response = await _httpClient.GetAsync("https://localhost:7103/UserRepository");
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}