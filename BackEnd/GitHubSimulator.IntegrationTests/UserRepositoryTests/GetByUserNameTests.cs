// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
// using GitHubSimulator.Dtos.UserRepositories;
//
// namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;
//
// public class GetByUserNameTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//     
//     public GetByUserNameTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task GetByUserName_ReturnsErrorWhenUserInternal()
//     {
//         // Arrange
//         var input = new GetUserRepositoriesByUserNameDto(new MailDto("TestEmail"));
//         
//         // Act
//         var response = await _httpClient.PostAsJsonAsync($"https://localhost:7103/UserRepository/GetByUserName", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
// }