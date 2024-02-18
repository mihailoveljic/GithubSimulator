namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryNameDto(
        string RepositoryName,
        string NewName
    );