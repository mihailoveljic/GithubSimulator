using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Dtos.Milestones;

namespace GitHubSimulator.IntegrationTests.MilestoneTests;

public class GetOpenOrClosedMilestonesTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetOpenOrClosedMilestonesTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task GetOpenMilestonesInvalidRepo_ReturnsInternal()
    {
        // Arrange
        var input = new GetOpenOrClosedMilestonesDto("TestRepoName", 0);
        
        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Milestone/getOpenOrClosed", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetClosedMilestonesInvalidRepo_ReturnsInternal()
    {
        // Arrange
        var input = new GetOpenOrClosedMilestonesDto("TestRepoName", 1);
        
        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Milestone/getOpenOrClosed", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
}