using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Specifications;

namespace GitHubSimulator.Core.Interfaces.Repositories;

public interface IMilestoneRepository
{
    Task<MilestoneWithIssues> GetMilestoneWithIssues(Specification<Issue> specification, Guid milestoneId);
    Task<IEnumerable<Milestone>> GetAll();
    Task<Milestone> Insert(Milestone milestone);
    Task<Maybe<Milestone>> Update(Milestone milestone);
    Task<bool> Delete(Guid id);
}
