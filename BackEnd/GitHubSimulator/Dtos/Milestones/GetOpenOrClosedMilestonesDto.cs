using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Dtos.Milestones;

public record GetOpenOrClosedMilestonesDto(
    Guid Id,
    int State
    );