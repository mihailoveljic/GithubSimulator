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

    public Task<IEnumerable<Issue>> GetAll() =>
        issueRepository.GetAll();

    public Task<Issue> Insert(Issue issue) => 
        issueRepository.Insert(issue);

    public Task<Maybe<Issue>> Update(Issue issue) => 
        issueRepository.Update(issue);
}
