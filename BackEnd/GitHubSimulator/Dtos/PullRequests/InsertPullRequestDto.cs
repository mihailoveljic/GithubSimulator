using GitHubSimulator.Core.Models.Abstractions;
using System.Reflection.Emit;

namespace GitHubSimulator.Dtos.PullRequests;

public record InsertPullRequestDto(
        Guid? source,
        Guid? target,
        string? assignee,
        string @base,
        string body,
        string head,
        string title,
        string repoName,
        string[]? assignees,
        Guid? issueId,
        Guid? milestoneId,
        Guid? repositoryId,
        bool isOpen,
        int? number,
        IEnumerable<Event>? events,
        IEnumerable<Guid>? LabelIds);
