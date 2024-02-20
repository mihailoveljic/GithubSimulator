using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.IntegrationTests.IssuesTests;

public class CreateIssueTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public CreateIssueTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task CreateIssueInvalidRepo_ReturnsInternal()
    {
        // Arrange
        var input = new InsertIssueDto("TestTitle", "TestDescription", new MailDto("TestMail"),
            "TestRepoName", Guid.NewGuid(), null, null);
        
        // Act
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Issue", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
}