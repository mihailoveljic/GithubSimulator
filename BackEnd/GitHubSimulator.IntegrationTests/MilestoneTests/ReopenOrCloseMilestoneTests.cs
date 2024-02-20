using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Milestones;

namespace GitHubSimulator.IntegrationTests.MilestoneTests;

public class ReopenOrCloseMilestoneTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public ReopenOrCloseMilestoneTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task ReopenMilestoneInvalidRepo_ReturnsNotFound()
    {
        // Arrange
        var input = new ReopenOrCloseMilestoneDto(new Guid(), 0);
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Milestone/reopenOrClose", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task CloseMilestoneInvalidRepo_ReturnsNotFound()
    {
        // Arrange
        var input = new ReopenOrCloseMilestoneDto(new Guid(), 1);
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Milestone/reopenOrClose", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}