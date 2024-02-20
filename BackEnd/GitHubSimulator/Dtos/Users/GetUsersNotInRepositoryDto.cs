namespace GitHubSimulator.Dtos.Users;

public record GetUsersNotInRepositoryDto(
        string RepositoryName,
        string SearchString
    );