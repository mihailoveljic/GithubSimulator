// using System.Net;
// using FluentAssertions;
//
// namespace GitHubSimulator.IntegrationTests.IssuesTests;
//
// public class DeleteIssueTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public DeleteIssueTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task DeleteIssueInvalidRepo_ReturnsInternal()
//     {
//         // Arrange
//         var issueId = Guid.NewGuid();
//         
//         // Act
//         var response = await _httpClient.DeleteAsync("https://localhost:7103/Issue?id=" + issueId);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
// }