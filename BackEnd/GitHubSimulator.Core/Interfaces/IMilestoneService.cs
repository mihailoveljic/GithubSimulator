using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces;

public interface IMilestoneService
{
    Task<IEnumerable<MilestoneWithIssues>> GetAllWithIssues(IEnumerable<Guid>? milestoneIds);
    Task<IEnumerable<Milestone>> GetAll();
}
