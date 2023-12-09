using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Repositories;
public interface IPullRequestRepository
{
    Task<IEnumerable<PullRequest>> GetAll();
    Task<Maybe<PullRequest>> GetById(Guid id);
    Task<PullRequest> Insert(PullRequest pullRequest);
    Task<Maybe<PullRequest>> Update(PullRequest pullRequest);
    Task<bool> Delete(Guid id);
}
