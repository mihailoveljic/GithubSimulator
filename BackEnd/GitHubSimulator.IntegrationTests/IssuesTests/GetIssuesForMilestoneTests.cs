// using System.Net;
// using FluentAssertions;
//
// namespace GitHubSimulator.IntegrationTests.IssuesTests;
//
// public class GetIssuesForMilestoneTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public GetIssuesForMilestoneTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task GetIssuesForMilestoneInvalidMilestoneId_ReturnsNotFound()
//     {
//         // Arrange
//         var milestoneId = Guid.NewGuid();
//         
//         // Act
//         var response = await _httpClient.GetAsync("https://localhost:7103/Issues/getIssuesForMilestone?milestoneId=" + milestoneId);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         errorMessage.Should().NotBeNull();
//     }
// }