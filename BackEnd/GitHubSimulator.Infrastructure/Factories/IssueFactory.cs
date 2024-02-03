using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Infrastructure.Factories;

public class IssueFactory
{
    public static Core.Models.Entities.Issue MapToDomain(Models.Issue issue) =>
        Core.Models.Entities.Issue.Create(
            issue.Title,
            issue.Description,
            Mail.Create(issue.AssigneeEmail),
            issue.RepositoryId,
            issue.MilestoneId,
            null,
            issue.Id);
}
