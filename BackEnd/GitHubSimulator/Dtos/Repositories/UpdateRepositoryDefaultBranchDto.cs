namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryDefaultBranchDto(
        string RepositoryOwner,
        string RepositoryName,
        string NewDefaultBranchName
    );