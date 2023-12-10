using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Labels;
using GitHubSimulator.Dtos.PullRequests;

namespace GitHubSimulator.Factories;

public class PullRequestFactory
{
    public PullRequest MapToDomain(InsertPullRequestDto dto) =>
        PullRequest.Create(
            dto.Source,
            dto.Target,
            dto.Events,
            dto.IssueId,
            dto.MilestoneId);

    public PullRequest MapToDomain(UpdatePullRequestDto dto) =>
        PullRequest.Create(
            dto.Source,
            dto.Target,
            dto.Events,
            dto.IssueId,
            dto.MilestoneId,
            dto.Id);
}
