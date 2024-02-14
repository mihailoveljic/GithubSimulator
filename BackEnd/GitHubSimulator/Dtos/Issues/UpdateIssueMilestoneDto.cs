namespace GitHubSimulator.Dtos.Issues;

public record UpdateIssueMilestoneDto(
    Guid Id, 
    Guid? MilestoneId
    );