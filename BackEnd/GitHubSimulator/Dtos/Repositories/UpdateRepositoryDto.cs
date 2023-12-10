namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryDto (
        Guid Id,
        string Name,
        string Description
    );