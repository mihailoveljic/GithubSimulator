using System.Net;
using FluentAssertions;

namespace GitHubSimulator.IntegrationTests.MilestoneTests;

public class DeleteMilestonesTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public DeleteMilestonesTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task DeleteMilestoneInvalidRepo_ReturnsNotFound()
    {
        // Arrange
        var milestoneId = Guid.NewGuid();
        
        // Act
        var response = await _httpClient.DeleteAsync("https://localhost:7103/Milestone?id=" + milestoneId);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}