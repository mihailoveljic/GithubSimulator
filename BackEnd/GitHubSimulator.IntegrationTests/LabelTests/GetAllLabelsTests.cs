using System.Net;
using FluentAssertions;

namespace GitHubSimulator.IntegrationTests.LabelTests;

public class GetAllLabelsTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllLabelsTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task GetAllLabels_ReturnsOk()
    {
        // Arrange
        
        // Act
        var response = await _httpClient.GetAsync("https://localhost:7103/Label/All");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}