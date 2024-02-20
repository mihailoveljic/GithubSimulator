// using System.Net;
// using FluentAssertions;
//
// namespace GitHubSimulator.IntegrationTests.MilestoneTests;
//
// public class GetAllMilestonesTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public GetAllMilestonesTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//
//     [Fact]
//     public async Task GetAllMilestones()
//     {
//         // Arrange
//         
//         // Act
//         var response = 
//             await _httpClient.GetAsync("https://localhost:7103/Milestone/GetAllMilestones");
//
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//     }
// }