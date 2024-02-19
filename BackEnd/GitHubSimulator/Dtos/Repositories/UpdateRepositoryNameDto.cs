namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryNameDto(
        string RepositoryOwner,
        string RepositoryName,
        string NewName
    );