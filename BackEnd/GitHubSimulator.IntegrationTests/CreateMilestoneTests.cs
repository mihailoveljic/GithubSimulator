using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Dtos.Milestones;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GitHubSimulator.IntegrationTests;

// It implements this interface so that a new instance of WebApplicationFactory is not created every time a test is ran 
public class CreateMilestoneTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly WebApplicationFactory<IApiMarker> _webApplicationFactory;

    public CreateMilestoneTests(WebApplicationFactory<IApiMarker> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }

    [Fact]
    public async Task GivenValidMilestone_CreatesMilestone()
    {
        // Arrange
        var httpClient = _webApplicationFactory.CreateClient();
        var milestone = 
            new MilestoneDto("Test", "Test", DateTime.Now.AddDays(1), State.Open, new Guid());

        // Act
        var response = await httpClient.PostAsJsonAsync("https://localhost:7103/Milestone", milestone);
        // Cannot use Milestone here, because the entity class does not have an empty constructor
        var createdMilestone = await response.Content.ReadFromJsonAsync<MilestoneDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        createdMilestone.Should().NotBeNull();
        createdMilestone!.Title.Should().Be("Test");
    }
}