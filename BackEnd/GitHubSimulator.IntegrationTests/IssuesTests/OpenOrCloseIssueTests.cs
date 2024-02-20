// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
//
// namespace GitHubSimulator.IntegrationTests.IssuesTests;
//
// public class OpenOrCloseIssueTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public OpenOrCloseIssueTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task OpenIssueInvalidRepo_ReturnsInternal()
//     {
//         // Arrange
//         var input = new OpenOrCloseIssueDto(Guid.NewGuid(), true);
//         
//         // Act
//         var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue/openOrClose", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
//     
//     [Fact]
//     public async Task CloseIssueInvalidRepo_ReturnsInternal()
//     {
//         // Arrange
//         var input = new OpenOrCloseIssueDto(Guid.NewGuid(), false);
//         
//         // Act
//         var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue/openOrClose", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
// }