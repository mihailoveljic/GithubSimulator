using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Repositories;

public interface IIssueRepository
{
    Task<IEnumerable<Issue>> GetAll();
    Task<Maybe<Issue>> GetById(Guid id);

    Task<Issue> Insert(Issue issue);
    Task<Maybe<Issue>> Update(Issue issue);
    Task<bool> Delete(Guid id);
}
