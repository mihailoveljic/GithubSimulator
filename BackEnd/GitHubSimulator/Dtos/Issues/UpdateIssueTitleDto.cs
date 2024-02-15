namespace GitHubSimulator.Dtos.Issues;

public record UpdateIssueTitleDto(
    Guid Id,
    string Title
    );