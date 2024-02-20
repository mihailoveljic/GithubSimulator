using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Dtos.Repositories;

public record UpdateRepositoryOwner(
        string RepositoryName,
        MailDto NewOwner
    );