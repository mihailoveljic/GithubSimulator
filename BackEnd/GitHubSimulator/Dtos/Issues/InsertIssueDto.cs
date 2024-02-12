using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Dtos.Issues;

public record InsertIssueDto(
    string Title,
    string Description,
    MailDto Assigne,
    Guid RepositoryId,
    Guid? MilestoneId,
    IEnumerable<Event>? Events,
    IEnumerable<Guid>? LabelIds);
