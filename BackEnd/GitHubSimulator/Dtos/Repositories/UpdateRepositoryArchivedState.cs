namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryArchivedState(
        string RepositoryOwner,
        string RepositoryName,
        bool IsArchived
    );