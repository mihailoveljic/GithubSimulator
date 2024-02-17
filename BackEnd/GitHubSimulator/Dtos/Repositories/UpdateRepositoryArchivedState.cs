namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryArchivedState(
        string RepositoryName,
        bool IsArchived
    );