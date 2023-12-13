using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryDto (
        Guid Id,
        string Name,
        string Description,
        Visibility Visibility
    );