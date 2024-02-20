using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Services;

public class PullRequestService : IPullRequestService
{
    private readonly IPullRequestRepository pullRequestRepository;

    public PullRequestService(IPullRequestRepository pullRequestRepository)
    {
        this.pullRequestRepository = pullRequestRepository;
    }
    public Task<bool> Delete(Guid id) =>
        pullRequestRepository.Delete(id);

    public Task<IEnumerable<PullRequest>> GetAll() =>
        pullRequestRepository.GetAll();

    public Task<IEnumerable<PullRequest>> GetAllForRepo(string repo) =>
        pullRequestRepository.GetAllForRepo(repo);


    public Task<Maybe<PullRequest>> GetById(Guid id) =>
        pullRequestRepository.GetById(id);

    public Task<PullRequest> Insert(PullRequest pullRequest) =>
        pullRequestRepository.Insert(pullRequest);

    public Task<IEnumerable<PullRequest>> SearchPullRequest(string searchString, string email, string repo) =>
        pullRequestRepository.SearchPullRequest(searchString, email, repo);

    public Task<Maybe<PullRequest>> Update(PullRequest pullRequest, string user) =>
        pullRequestRepository.Update(pullRequest, user);

    public Task<Maybe<PullRequest>> UpdateIsOpen(int index, bool isOpen) =>
        pullRequestRepository.UpdateIsOpen(index, isOpen);
}
