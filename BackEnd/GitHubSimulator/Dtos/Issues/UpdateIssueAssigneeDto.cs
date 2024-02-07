namespace GitHubSimulator.Dtos.Issues;

public record UpdateIssueAssigneeDto(
    Guid Id,
    MailDto? Assignee
    );