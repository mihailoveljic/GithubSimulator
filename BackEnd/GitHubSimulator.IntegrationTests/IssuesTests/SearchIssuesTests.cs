// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
//
// namespace GitHubSimulator.IntegrationTests.IssuesTests;
//
// public class SearchIssuesTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public SearchIssuesTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task SearchIssuesInvalidRepo_ReturnsInternal()
//     {
//         // Arrange
//         var repoName = "TestRepoName";
//         var input = new SearchIssuesDto("TestSearchString");
//         
//         // Act
//         var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Issue/searchIssues/" + repoName, input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
// }