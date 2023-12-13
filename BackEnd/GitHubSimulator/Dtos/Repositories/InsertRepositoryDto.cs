using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Dtos.Repositories;

public record InsertRepositoryDto(
        string Name,
        string Description,
        Visibility Visibility
    );