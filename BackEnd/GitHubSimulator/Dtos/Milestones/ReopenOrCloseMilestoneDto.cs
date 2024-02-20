namespace GitHubSimulator.Dtos.Milestones;
public record ReopenOrCloseMilestoneDto(
    Guid Id,
    int State
    );