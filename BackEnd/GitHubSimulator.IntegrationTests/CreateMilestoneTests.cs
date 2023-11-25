using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Dtos.Milestones;

namespace GitHubSimulator.IntegrationTests;

// It implements this interface so that a new instance of WebApplicationFactory is not created every time a test is ran 
public class CreateMilestoneTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    
    public CreateMilestoneTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }

    [Fact]
    public async Task GivenValidMilestone_CreatesMilestone()
    {
        // Arrange
        var milestone = 
            new MilestoneDto("Test", "Test", DateTime.Now.AddDays(1), State.Open, new Guid());

        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Milestone", milestone);
        // Cannot use Milestone here, because the entity class does not have an empty constructor
        var createdMilestone = await response.Content.ReadFromJsonAsync<MilestoneDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        createdMilestone.Should().NotBeNull();
        createdMilestone!.Title.Should().Be("Test");
    }
    
    [Fact]
    public async Task GivenInvalidMilestone_ReturnsError()
    {
        // Arrange
        var milestone = 
            new MilestoneDto("", "Test", DateTime.Now.AddDays(1), State.Open, new Guid());

        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Milestone", milestone);
        var errorMessage = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errorMessage.Should().NotBeNull();
        errorMessage.Should().Contain("Title must not be empty!");
    }
}