namespace GitHubSimulator.Dtos.Issues;

public record UpdateIssueLabelsDto(
    List<Guid> LabelIds
    );