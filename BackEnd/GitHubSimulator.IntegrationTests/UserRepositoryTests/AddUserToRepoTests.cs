// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
// using GitHubSimulator.Dtos.UserRepositories;
//
// namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;
//
// public class AddUserToRepoTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//     
//     public AddUserToRepoTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task AddUserToRepo_ReturnsErrorWhenRepositoryNotFound()
//     {
//         // Arrange
//         var user = new AddUserToRepositoryDto(new MailDto("TestEmail"), "TestRepoName");
//         
//         // Act
//         var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/AddUserToRepo", user);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         errorMessage.Should().NotBeNull();
//     }
// }