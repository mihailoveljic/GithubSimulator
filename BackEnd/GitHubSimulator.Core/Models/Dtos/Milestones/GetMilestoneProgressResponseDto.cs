namespace GitHubSimulator.Core.Models.Dtos.Milestones;

public record GetMilestoneProgressResponseDto(
    int OpenIssueCounter,
    int ClosedIssueCounter,
    double Progress
    );