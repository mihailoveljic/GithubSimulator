namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryVisibilityDto(
        string RepositoryName,
        bool IsPrivate
    );