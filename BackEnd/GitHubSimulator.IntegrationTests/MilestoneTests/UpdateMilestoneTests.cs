using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Milestones;

namespace GitHubSimulator.IntegrationTests.MilestoneTests;

public class UpdateMilestoneTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public UpdateMilestoneTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task UpdateMilestoneInvalidMilestone_ReturnsNotFound()
    {
        // Arrange
        var input = new UpdateMilestoneDto(Guid.NewGuid(), "TestMilestone", "TestDescription", 
            DateTime.Now.AddDays(7), 0, Guid.NewGuid());
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Milestone", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}