using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;

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

    public async Task<Issue> GetById(Guid id)
    {
        return await issueRepository.GetById(id);
    }

    public Task<IEnumerable<Issue>> GetAll() =>
        issueRepository.GetAll();

    public Task<Maybe<Issue>> GetById(Guid id) =>
        issueRepository.GetById(id);

    public Task<Issue> Insert(Issue issue) => 
        issueRepository.Insert(issue);

    public Task<Maybe<Issue>> Update(Issue issue) => 
        issueRepository.Update(issue);
}
