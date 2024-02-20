// using System.Net;
// using FluentAssertions;
//
// namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;
//
// public class GetAuthenticatedUserRepositoryRoleTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//     
//     public GetAuthenticatedUserRepositoryRoleTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task GetAuthenticatedUserRepositoryRole_ReturnsErrorWhenRepositoryNotFound()
//     {
//         // Arrange
//         var repoName = "TestRepoName";
//         
//         // Act
//         var response = await _httpClient.GetAsync($"https://localhost:7103/UserRepository/GetAuthUserRole{repoName}");
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         errorMessage.Should().NotBeNull();
//     }
// }