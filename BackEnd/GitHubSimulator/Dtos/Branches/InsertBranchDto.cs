namespace GitHubSimulator.Dtos.Branches;

public record InsertBranchDto (
    string New_Branch_Name,
    string Old_Branch_Name,
    string Old_Ref_Name,
    Guid RepositoryId,
    Guid? IssueId
);