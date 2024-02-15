using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Core.Specifications.Milestones;

namespace GitHubSimulator.Core.Services;

public class MilestoneService : IMilestoneService
{
    private readonly IMilestoneRepository repository;
    private readonly IIssueRepository _issueRepository;
    
    public MilestoneService(IMilestoneRepository repository, IIssueRepository issueRepository)
    {
        this.repository = repository;
        _issueRepository = issueRepository;
    }

    public Task<bool> Delete(Guid id) =>
        repository.Delete(id);

    public async Task<IEnumerable<Milestone>> GetAllMilestonesForRepository(Guid repoId)
        => await repository.GetAllMilestonesForRepository(repoId);

    public async Task<Maybe<Milestone>> ReopenOrCloseMilestone(Guid id, int state)
        => await repository.ReopenOrCloseMilestone(id, state);

    public async Task<IEnumerable<Milestone>> GetOpenOrClosedMilestones(Guid id, int state)
        => await repository.GetOpenOrClosedMilestones(id, state);

    public async Task<IEnumerable<Milestone>> GetAll() =>
        await repository.GetAll();

    public async Task<GetMilestoneProgressResponseDto> GetMilestoneProgress(Guid milestoneId)
    {
        var allIssuesForMilestone 
            = await _issueRepository.GetIssuesForMilestone(milestoneId);

        var openIssueCounter = 0;
        var closedIssueCounter = 0;

        foreach (var issue in allIssuesForMilestone)
        {
            if (issue.IsOpen) openIssueCounter++;
            else closedIssueCounter++;
        }

        return (openIssueCounter + closedIssueCounter != 0)  
            ? new GetMilestoneProgressResponseDto(openIssueCounter, closedIssueCounter, 
                (double)closedIssueCounter / (openIssueCounter + closedIssueCounter) * 100) 
            : new GetMilestoneProgressResponseDto(openIssueCounter, closedIssueCounter, 0);
    }

    public Task<Maybe<Milestone>> GetById(Guid id) =>
        repository.GetById(id);

    public async Task<IEnumerable<MilestoneWithIssues>> GetAllWithIssues(IEnumerable<Guid>? milestoneIds)
    {
        List<MilestoneWithIssues> milestonesWithIssues = new();
        if(milestoneIds is not null) 
        {
            foreach (var milestoneId in milestoneIds)
            {
                var milestoneWithIssues = await repository.GetMilestoneWithIssues(new MilestoneWithIssuesSpecification(milestoneId), milestoneId);
                milestonesWithIssues.Add(milestoneWithIssues);
            }
            return milestonesWithIssues;
        }
        else
        {
            var allMilestoneIds = (await repository.GetAll()).Select(x => x.Id);

            foreach (var milestoneId in allMilestoneIds)
            {
                var milestoneWithIssues = await repository.GetMilestoneWithIssues(new MilestoneWithIssuesSpecification(milestoneId), milestoneId);
                milestonesWithIssues.Add(milestoneWithIssues);
            }
            return milestonesWithIssues;
        }
    }

    public async Task<Milestone> Insert(Milestone milestone) =>
        await repository.Insert(milestone);

    public Task<CSharpFunctionalExtensions.Maybe<Milestone>> Update(Milestone milestone) =>
        repository.Update(milestone);
}
