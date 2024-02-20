using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Dtos.UserRepositories;

public record GetUserRepositoriesByRepositoryNameAltResultDto(
        Guid Id,
        string RepositoryName,
        string UserEmail,
        UserRepositoryRole UserRepositoryRole
    );