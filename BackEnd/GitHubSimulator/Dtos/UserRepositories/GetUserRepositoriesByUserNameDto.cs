using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Dtos.UserRepositories;

public record GetUserRepositoriesByUserNameDto(
        MailDto User
    );