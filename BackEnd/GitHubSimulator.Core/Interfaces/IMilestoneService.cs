using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces;

public interface IMilestoneService
{
    Task<IEnumerable<MilestoneWithIssues>> GetAllWithIssues(IEnumerable<Guid>? milestoneIds);
    Task<IEnumerable<Milestone>> GetAll();
    Task<Milestone> Insert(Milestone milestone);
    Task<Maybe<Milestone>> Update(Milestone milestone);
    Task<bool> Delete(Guid id);
}
