using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IPullRequestService
{
    Task<IEnumerable<PullRequest>> GetAll();
    Task<IEnumerable<PullRequest>> GetAllForRepo(string repo);
    Task<Maybe<PullRequest>> GetById(Guid id);
    Task<PullRequest> Insert(PullRequest pullRequest);
    Task<Maybe<PullRequest>> Update(PullRequest pullRequest, string user);
    Task<Maybe<PullRequest>> UpdateIsOpen(int index, bool isOpen);
    Task<IEnumerable<PullRequest>> SearchPullRequest(string searchString, string email, string repo);
    Task<bool> Delete(Guid id);
}
