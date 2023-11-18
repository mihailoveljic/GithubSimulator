using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Specifications;

namespace GitHubSimulator.Core.Interfaces;

public interface IMilestoneRepository
{
    Task<MilestoneWithIssues> GetMilestoneWithIssues(Specification<Issue> specification, Guid milestoneId);
    Task<IEnumerable<Milestone>> GetAll();
}
