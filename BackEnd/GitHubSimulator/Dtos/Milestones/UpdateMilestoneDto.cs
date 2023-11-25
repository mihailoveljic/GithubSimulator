using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Dtos.Milestones;

public record UpdateMilestoneDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    State State,
    Guid RepositoryId);
