// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
//
// namespace GitHubSimulator.IntegrationTests.IssuesTests;
//
// public class UpdateTitleTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public UpdateTitleTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task UpdateTitleInvalidInput_ReturnsNotFound()
//     {
//         // Arrange
//         var input = new UpdateIssueTitleDto(Guid.NewGuid(), "TestTitle");
//         
//         // Act
//         var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issues/updateTitle", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         errorMessage.Should().NotBeNull();
//     }
// }