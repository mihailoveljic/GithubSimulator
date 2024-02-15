using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IMilestoneService
{
    Task<IEnumerable<MilestoneWithIssues>> GetAllWithIssues(IEnumerable<Guid>? milestoneIds);
    Task<IEnumerable<Milestone>> GetAll();
    Task<GetMilestoneProgressResponseDto> GetMilestoneProgress(Guid milestoneId);
    Task<Maybe<Milestone>> GetById(Guid id);
    Task<Milestone> Insert(Milestone milestone);
    Task<Maybe<Milestone>> Update(Milestone milestone);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<Milestone>> GetAllMilestonesForRepository(Guid repoId);
    Task<Maybe<Milestone>> ReopenOrCloseMilestone(Guid id, int state);
    Task<IEnumerable<Milestone>> GetOpenOrClosedMilestones(Guid id, int state);
}
