using System.Net;
using FluentAssertions;

namespace GitHubSimulator.IntegrationTests.IssuesTests;

public class GetAllIssuesTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllIssuesTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task GetAllIssuesInvalidCache_ReturnsNotFound()
    {
        // Arrange
        
        // Act
        var response = await _httpClient.GetAsync("https://localhost:7103/Issues/All");
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}