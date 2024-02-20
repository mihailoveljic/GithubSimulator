namespace GitHubSimulator.IntegrationTests.MilestoneTests;

public class GetOpenOrClosedMilestonesTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetOpenOrClosedMilestonesTests(ApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.HttpClient;
    }
    
    [Fact]
    public async Task GetOpenMilestones_ReturnsOpenMilestones()
    {
        // Arrange
        var response = await _httpClient.GetAsync("https://localhost:7103/Milestone/getOpenOrClosed");
        var milestones = await response.Content.ReadFromJsonAsync<List<MilestoneDto>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        milestones.Should().NotBeNull();
        milestones.Should().NotBeEmpty();
        milestones.Should().OnlyContain(m => m.State == State.Open);
    }
    
    [Fact]
    public async Task GetClosedMilestones_ReturnsClosedMilestones()
    {
        // Arrange
        var response = await _httpClient.GetAsync("https://localhost:7103/Milestone/GetClosed");
        var milestones = await response.Content.ReadFromJsonAsync<List<MilestoneDto>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        milestones.Should().NotBeNull();
        milestones.Should().NotBeEmpty();
        milestones.Should().OnlyContain(m => m.State == State.Closed);
    }
}