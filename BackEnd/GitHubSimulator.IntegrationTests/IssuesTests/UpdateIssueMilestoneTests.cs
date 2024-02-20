using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.IntegrationTests.IssuesTests;

public class UpdateIssueMilestoneTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public UpdateIssueMilestoneTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task assignMilestoneInvalidRepo_ReturnsInternal()
    {
        // Arrange
        var input = new UpdateIssueMilestoneDto(Guid.NewGuid(), new Guid());
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue/updateMilestone", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
    
    [Fact]
    public async Task unassignMilestoneInvalidRepo_ReturnsInternal()
    {
        // Arrange
        var input = new UpdateIssueMilestoneDto(Guid.NewGuid(), null);
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue/updateMilestone", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
}