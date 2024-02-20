using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.IntegrationTests.IssuesTests;

public class UpdateIssueTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public UpdateIssueTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task UpdateIssueInvalidInput_ReturnsInternal()
    {
        // Arrange
        var input = new UpdateIssueDto(Guid.NewGuid(), "TestTitle", "TestDescription", 
            DateTime.Now, new MailDto("TestMail"), Guid.NewGuid(), Guid.NewGuid(), null);
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue", input);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
}