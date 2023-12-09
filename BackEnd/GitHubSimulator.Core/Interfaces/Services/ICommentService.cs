using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface ICommentService
{
    Task<Comment> GetById(Guid id);
    Task<IEnumerable<Comment>> GetAll();
    Task<Comment> Insert(Comment comment);
    Task<Maybe<Comment>> Update(Comment comment);
    Task<bool> Delete(Guid id);
}