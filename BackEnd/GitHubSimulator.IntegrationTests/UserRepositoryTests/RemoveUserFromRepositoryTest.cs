// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
// using GitHubSimulator.Dtos.UserRepositories;
//
// namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;
//
// public class RemoveUserFromRepositoryTest : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//     
//     public RemoveUserFromRepositoryTest(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task RemoveUserFromRepository_ReturnsErrorWhenRepositoryInternal()
//     {
//         // Arrange
//         var input = new AddUserToRepositoryDto(new MailDto("TestEmail"), "TestRepository");
//         
//         // Act
//         var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/UserRepository/RemoveUserFromRepo", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
// }