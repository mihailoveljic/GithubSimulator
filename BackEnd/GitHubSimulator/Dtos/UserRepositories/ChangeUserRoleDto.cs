using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Dtos.UserRepositories;

public record ChangeUserRoleDto(
        MailDto User,
        string RepositoryName,
        UserRepositoryRole NewRole
    );