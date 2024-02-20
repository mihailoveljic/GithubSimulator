// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Issues;
//
// namespace GitHubSimulator.IntegrationTests.IssuesTests;
//
// public class UpdateIssueAssigneeTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public UpdateIssueAssigneeTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task assignUserToIssueInvalidRepo_ReturnsInternal()
//     {
//         // Arrange
//         var input = new UpdateIssueAssigneeDto(Guid.NewGuid(), new MailDto("TestMail"));
//         
//         // Act
//         var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue/updateAssignee", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
//     
//     [Fact]
//     public async Task unassignUserToIssueInvalidRepo_ReturnsInternal()
//     {
//         // Arrange
//         var input = new UpdateIssueAssigneeDto(Guid.NewGuid(), null);
//         
//         // Act
//         var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/Issue/updateAssignee", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//         errorMessage.Should().NotBeNull();
//     }
// }