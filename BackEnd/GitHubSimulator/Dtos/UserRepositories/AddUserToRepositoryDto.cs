using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Dtos.UserRepositories;

public record AddUserToRepositoryDto(
        MailDto User,
        string RepositoryName
    );