// using System.Net;
// using System.Net.Http.Json;
// using FluentAssertions;
// using GitHubSimulator.Dtos.Labels;
//
// namespace GitHubSimulator.IntegrationTests.LabelTests;
//
// public class SearchLabelsTests : IClassFixture<ApiFactory>
// {
//     private readonly HttpClient _httpClient;
//
//     public SearchLabelsTests(ApiFactory webApplicationFactory)
//     {
//         _httpClient = webApplicationFactory.HttpClient;
//     }
//     
//     [Fact]
//     public async Task SearchLabelsReturnsOk()
//     {
//         // Arrange
//         var input = new SearchLabelsDto("TestSearchString");
//         
//         // Act
//         var response = await _httpClient.PostAsJsonAsync("https://localhost:7103/Label/searchLabels", input);
//         var errorMessage = await response.Content.ReadAsStringAsync();
//         
//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.OK);
//     }
// }