using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Dtos.Milestones;

public record InsertMilestoneDto(
    string Title, 
    string Description, 
    DateTime DueDate, 
    State State, 
    Guid RepositoryId);

