namespace GitHubSimulator.Dtos.Branches;

public record UpdateBranchDto(
    Guid Id,
    string Name,
    Guid? IssueId
);