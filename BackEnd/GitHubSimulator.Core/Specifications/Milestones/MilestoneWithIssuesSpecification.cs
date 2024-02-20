using GitHubSimulator.Core.Models.Entities;
using System.Linq.Expressions;

namespace GitHubSimulator.Core.Specifications.Milestones;

sealed class MilestoneWithIssuesSpecification : Specification<Issue>
{
    private readonly Guid milestoneId;

    public MilestoneWithIssuesSpecification(Guid milestoneId)
    {
        this.milestoneId = milestoneId;
    }

    public override Expression<Func<Issue, bool>> ToExpression()
        => issue => issue.MilestoneId.Equals(milestoneId);
}
