using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Dtos.Issues;

public record UpdateIssueDto(
    Guid Id, 
    string Title, 
    string Description, 
    DateTime CreatedAt,
    MailDto Assignee,
    Guid RepositoryId, 
    Guid? MilestoneId,
    IEnumerable<Event>? Events);
