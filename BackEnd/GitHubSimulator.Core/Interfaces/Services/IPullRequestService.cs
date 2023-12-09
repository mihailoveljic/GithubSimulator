using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IPullRequestService
{
    Task<IEnumerable<PullRequest>> GetAll();
    Task<Maybe<PullRequest>> GetById(Guid id);
    Task<PullRequest> Insert(PullRequest pullRequest);
    Task<Maybe<PullRequest>> Update(PullRequest pullRequest);
    Task<bool> Delete(Guid id);
}
