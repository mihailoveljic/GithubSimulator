using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Labels;

namespace GitHubSimulator.IntegrationTests.LabelTests;

public class UpdateLabelTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public UpdateLabelTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task UpdateLabelInvalidRepo_ReturnsNotFound()
    {
        // Arrange
        var input = new UpdateLabelDto(Guid.NewGuid(), "TestLabelName", "TestLabelDescription", "TestLabelColor");
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Label", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}