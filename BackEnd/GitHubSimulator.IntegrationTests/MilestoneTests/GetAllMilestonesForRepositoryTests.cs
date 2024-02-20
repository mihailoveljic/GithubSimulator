using System.Net;
using FluentAssertions;

namespace GitHubSimulator.IntegrationTests.MilestoneTests;

public class GetAllMilestonesForRepositoryTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllMilestonesForRepositoryTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }

    [Fact]
    public async Task GetAllMilestonesForRepositoryNotFoundRepoReturnsNotFound()
    {
        // Arrange
        var repoName = "TestRepository";

        var response = await _httpClient.GetAsync("https://localhost:7103/AllForRepo/" + repoName);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        errorMessage.Should().NotBeNull();
    }
}