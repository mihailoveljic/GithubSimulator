namespace GitHubSimulator.Dtos.Issues;
public record OpenOrCloseIssueDto(
    Guid Id,
    bool IsOpen
    );