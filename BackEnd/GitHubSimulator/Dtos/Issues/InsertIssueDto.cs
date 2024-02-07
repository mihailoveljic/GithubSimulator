using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Dtos.Issues;

public record InsertIssueDto(
    string Title,
    string Description,
    MailDto Assigne,
    Guid RepositoryId,
    Guid? MilestoneId,
    IEnumerable<Event>? Events);
