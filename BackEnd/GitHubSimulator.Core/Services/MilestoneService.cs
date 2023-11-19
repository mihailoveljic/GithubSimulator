using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Specifications.Milestones;

namespace GitHubSimulator.Core.Services;

public class MilestoneService : IMilestoneService
{
    private readonly IMilestoneRepository repository;

    public MilestoneService(IMilestoneRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<Milestone>> GetAll() =>
        await repository.GetAll();

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
}
