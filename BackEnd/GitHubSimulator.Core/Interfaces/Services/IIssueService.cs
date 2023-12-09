using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IIssueService
{
    Task<Issue> GetById(Guid id);
    Task<IEnumerable<Issue>> GetAll();
    Task<Issue> Insert(Issue issue);
    Task<Maybe<Issue>> Update(Issue issue);
    Task<bool> Delete(Guid id);
}
