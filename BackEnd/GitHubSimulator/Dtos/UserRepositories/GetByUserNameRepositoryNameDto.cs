namespace GitHubSimulator.Dtos.UserRepositories;

public record GetByUserNameRepositoryNameDto(
        string UserName,
        string RepositoryName
    );