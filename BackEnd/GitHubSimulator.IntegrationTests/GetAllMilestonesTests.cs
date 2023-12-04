using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Milestones;

namespace GitHubSimulator.IntegrationTests;

public class GetAllMilestonesTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllMilestonesTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }

    [Fact]
    public async Task GetAllMilestones()
    {
        // Arrange
        
        // Act
        var response = 
            await _httpClient.GetAsync("https://localhost:7103/Milestone/GetAllMilestones");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}