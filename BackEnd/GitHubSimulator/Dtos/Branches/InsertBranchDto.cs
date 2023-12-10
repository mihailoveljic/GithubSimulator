namespace GitHubSimulator.Dtos.Branches;

public record InsertBranchDto (
    string Name,
    Guid RepositoryId,
    Guid? IssueId
);