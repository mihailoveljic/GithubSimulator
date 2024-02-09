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

    public async Task<IEnumerable<Issue>> GetIssuesForMilestone(Guid milestoneId) 
        => await issueRepository.GetIssuesForMilestone(milestoneId);

    public Task<Maybe<Issue>> GetById(Guid id) =>
        issueRepository.GetById(id);

    public Task<Issue> Insert(Issue issue) => 
        issueRepository.Insert(issue);

    public Task<Maybe<Issue>> Update(Issue issue) => 
        issueRepository.Update(issue);

    public async Task<Maybe<Issue>> UpdateIssueTitle(Guid id, string newTitle, string email)
        => await issueRepository.UpdateIssueTitle(id, newTitle, email);
    
    public async Task<Maybe<Issue>> UpdateIssueMilestone(Guid id, Guid? milestoneId, string email)
        => await issueRepository.UpdateIssueMilestone(id, milestoneId, email);

    public async Task<Maybe<Issue>> UpdateIssueAssignee(Guid id, string? assignee, string email)
        => await issueRepository.UpdateIssueAssignee(id, assignee, email);
    
    public async Task<Maybe<Issue>> OpenOrCloseIssue(Guid id, bool isOpen, string email) 
        => await issueRepository.OpenOrCloseIssue(id, isOpen, email);
}
