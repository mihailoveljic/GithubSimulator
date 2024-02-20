using System;
using System.Data;
using Amazon.Auth.AccessControlPolicy;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Labels;
using GitHubSimulator.Dtos.PullRequests;
using GitHubSimulator.Infrastructure.Models;
using Label = GitHubSimulator.Core.Models.Entities.Label;

namespace GitHubSimulator.Factories;

public class PullRequestFactory
{
    public PullRequest MapToDomain(InsertPullRequestDto dto, IEnumerable<Label> newLabels, string username, int number) =>
        PullRequest.Create(
            dto.source,
            dto.target,
            dto.assignee,
            dto.@base,
            dto.body,
            dto.head,
            dto.title,
            dto.repoName,
            dto.isOpen,
            username,
            DateTime.Now,
            dto.assignees,
            number,
            dto.issueId,
            dto.milestoneId,
            dto.events,
            newLabels
        );

    public PullRequest MapToDomain(InsertPullRequestDto dto, IEnumerable<Label> newLabels, Guid id, string username) =>
     PullRequest.Create(
         dto.source,
         dto.target,
         dto.assignee,
         dto.@base,
         dto.body,
         dto.head,
         dto.title,
         dto.repoName,
         dto.isOpen,
         username,
         DateTime.Now,
         dto.assignees,
         dto.number,
         dto.issueId,
         dto.milestoneId,
         dto.events,
         newLabels,
         id
     );
}
