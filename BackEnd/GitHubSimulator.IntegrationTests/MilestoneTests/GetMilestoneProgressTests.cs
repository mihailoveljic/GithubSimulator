using System.Net;
using FluentAssertions;

namespace GitHubSimulator.IntegrationTests.MilestoneTests;

public class GetMilestoneProgressTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetMilestoneProgressTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }

    [Fact]
    public async Task GetProgressInvalidMilestoneIdReturnsNotFound()
    {
        // Arrange
        var milestoneId = new Guid();
        
        // Act
        var response = await _httpClient.GetAsync("https://localhost:7103/Milestone?milestoneId=" + milestoneId);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}