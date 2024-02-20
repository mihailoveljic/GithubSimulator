using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Labels;

namespace GitHubSimulator.IntegrationTests.LabelTests;

public class CreateLabelTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public CreateLabelTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task CreateLabel_ReturnsCreated()
    {
        // Arrange
        var input = new InsertLabelDto("TestLabel", "TestDescription", "TestColor");
        
        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Label", input);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}