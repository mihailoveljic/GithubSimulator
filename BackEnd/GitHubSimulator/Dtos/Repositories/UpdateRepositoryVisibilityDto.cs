namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryVisibilityDto(
        string RepositoryOwner,
        string RepositoryName,
        bool IsPrivate
    );