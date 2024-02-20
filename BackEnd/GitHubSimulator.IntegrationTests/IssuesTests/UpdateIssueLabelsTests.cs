// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
//
// namespace GitHubSimulator.IntegrationTests.IssuesTests;
//
// public class UpdateIssueLabelsTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public UpdateIssueLabelsTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task UpdateIssueLabelsInvalidRepo_ReturnsInternal()
//     {
//         // Arrange
//         var issueId = Guid.NewGuid();
//         var input = new UpdateIssueLabelsDto(new List<Guid>());
//         
//         // Act
//         var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue/updateLabels?issueId=" + issueId, input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
// }