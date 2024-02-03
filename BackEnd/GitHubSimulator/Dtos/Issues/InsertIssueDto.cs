using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Dtos.Issues;

public record InsertIssueDto(
    string Title,
    string Description,
    DateTime CreatedAt,
    MailDto Assignee,
    Guid RepositoryId,
    Guid? MilestoneId,
    IEnumerable<Event>? Events);
