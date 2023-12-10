using GitHubSimulator.Core.Models.Abstractions;

namespace GitHubSimulator.Dtos.PullRequests;

public record UpdatePullRequestDto(
    Guid Id,
    Guid Source,
    Guid Target,
    Guid Destination,
    Guid MilestoneId,
    Guid IssueId,
    IEnumerable<Event>? Events);
