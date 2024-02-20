// using System.Net;
// using FluentAssertions;
//
// namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;
//
// public class GetAuthenticatedUserRepositoryTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//     
//     public GetAuthenticatedUserRepositoryTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//
//     [Fact]
//     public async Task GetAuthenticatedUserRepository_ReturnsErrorWhenRepositoryNotFound()
//     {
//         // Arrange
//         
//         // Act
//         var response = await _httpClient.GetAsync("https://localhost:7103/GetAuthenticatedUserUserRepos");
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         errorMessage.Should().NotBeNull();
//     }
// }