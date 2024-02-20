// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.UserRepositories;
//
// namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;
//
// public class GetByRepoNameTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//     
//     public GetByRepoNameTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task GetByRepoName_ReturnsErrorWhenRepositoryNotFound()
//     {
//         // Arrange
//         var input = new GetUserRepositoriesByRepositoryNameDto("TestRepoName");
//         
//         // Act
//         var response = await _httpClient.PostAsJsonAsync($"https://localhost:7103/UserRepository/GetByRepoName", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         errorMessage.Should().NotBeNull();
//     }
// }