using System.Reflection.Emit;
using GitHubSimulator.Core.Models.Abstractions;

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
        bool isOpen,
        int? number,
        IEnumerable<Event>? events,
        IEnumerable<Guid>? LabelIds);
