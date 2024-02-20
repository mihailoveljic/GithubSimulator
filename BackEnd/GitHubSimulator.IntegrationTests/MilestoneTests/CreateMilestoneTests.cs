using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Dtos.Milestones;

namespace GitHubSimulator.IntegrationTests.MilestoneTests;

// It implements this interface so that a new instance of WebApplicationFactory is not created every time a test is ran 
public class CreateMilestoneTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    
    public CreateMilestoneTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }

    [Fact]
    public async Task CreateMilestone_ReturnsErrorWhenRepositoryNotFound()
    {
        // Arrange
        var milestone = 
            new InsertMilestoneDto("Test", "Test", DateTime.Now.AddDays(1), State.Open, "");

        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Milestone", milestone);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GivenInvalidMilestone_ReturnsError()
    {
        // Arrange
        var milestone = 
            new InsertMilestoneDto("", "Test", DateTime.Now.AddDays(1), State.Open, "");

        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Milestone", milestone);
        var errorMessage = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
}