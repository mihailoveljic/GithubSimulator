using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Services;

public class IssueService : IIssueService
{
    private readonly IIssueRepository issueRepository;

    public IssueService(IIssueRepository issueRepository)
    {
        this.issueRepository = issueRepository;
    }

    public Task<bool> Delete(Guid id) =>
        issueRepository.Delete(id);

    public Task<IEnumerable<Issue>> GetAll() =>
        issueRepository.GetAll();

    public Task<Maybe<Issue>> GetById(Guid id) =>
        issueRepository.GetById(id);

    public Task<Issue> Insert(Issue issue) => 
        issueRepository.Insert(issue);

    public Task<Maybe<Issue>> Update(Issue issue) => 
        issueRepository.Update(issue);

    public async Task<Maybe<Issue>> UpdateIssueTitle(Guid id, string newTitle)
        => await issueRepository.UpdateIssueTitle(id, newTitle);
    
    public async Task<Maybe<Issue>> UpdateIssueMilestone(Guid id, Guid? milestoneId)
        => await issueRepository.UpdateIssueMilestone(id, milestoneId);

    public async Task<Maybe<Issue>> UpdateIssueAssignee(Guid id, string? assignee)
        => await issueRepository.UpdateIssueAssignee(id, assignee);
}
