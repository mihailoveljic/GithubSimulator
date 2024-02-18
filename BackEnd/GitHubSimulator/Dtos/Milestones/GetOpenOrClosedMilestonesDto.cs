using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Dtos.Milestones;

public record GetOpenOrClosedMilestonesDto(
    string RepoName,
    int State
    );