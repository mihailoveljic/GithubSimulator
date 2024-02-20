using System.Net;
using FluentAssertions;

namespace GitHubSimulator.IntegrationTests.LabelTests;

public class DeleteLabelTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public DeleteLabelTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task DeleteLabelInvalidId_ReturnsNotFound()
    {
        // Arrange
        var input = new Guid("00000000-0000-0000-0000-000000000000");
        
        // Act
        var response = await _httpClient.DeleteAsync($"https://localhost:7103/Label/{input}" );
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}