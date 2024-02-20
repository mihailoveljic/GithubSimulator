using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Dtos.Issues;
using GitHubSimulator.Dtos.UserRepositories;
using StackExchange.Redis;

namespace GitHubSimulator.IntegrationTests.UserRepositoryTests;

public class ChangeUserRoleTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;
    
    public ChangeUserRoleTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task ChangeUserRole_ReturnsErrorWhenUserNotFound()
    {
        // Arrange
        var userRole = new ChangeUserRoleDto(new MailDto("TestEmail"), "TestRepositoryName", UserRepositoryRole.Read);
        
        // Act
        var response = await _httpClient.PutAsJsonAsync("https://localhost:7103/UserRepository/ChangeUserRole", userRole);
        var errorMessage = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        errorMessage.Should().NotBeNull();
    }
}