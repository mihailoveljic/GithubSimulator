using CSharpFunctionalExtensions;
using GitHubSimulator.Core.BuildingBlocks.Exceptions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPullRequestRepository _requestRepository;
    private readonly IIssueRepository _issueRepository;

    public CommentService(ICommentRepository commentRepository, IPullRequestRepository requestRepository, IIssueRepository issueRepository)
    {
        _commentRepository = commentRepository;
        _requestRepository = requestRepository;
        _issueRepository = issueRepository;
    }

    public async Task<Comment> GetById(Guid id)
    {
        return await _commentRepository.GetById(id);
    }

    public async Task<IEnumerable<Comment>> GetAll()
    {
        return await _commentRepository.GetAll();
    }

    public async Task<Comment> Insert(Comment comment)
    {
        await CheckIfTaskExists(comment);
        return await _commentRepository.Insert(comment);
    }

    private async Task CheckIfTaskExists(Comment comment)
    {
        var relatedIssue = await _issueRepository.GetById(comment.TaskId);
        var relatedPullRequest = await _requestRepository.GetById(comment.TaskId);
        if (relatedIssue.Equals(Maybe.None) && relatedPullRequest.Equals(Maybe.None))
        {
            throw new InvalidTaskForCommentException();
        }
    }

    public async Task<Maybe<Comment>> Update(Comment comment)
    {
        return await _commentRepository.Update(comment);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _commentRepository.Delete(id);
    }
}