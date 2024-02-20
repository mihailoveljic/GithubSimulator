using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<Comment> GetById(Guid id);
    Task<IEnumerable<Comment>> GetAll();
    Task<Comment> Insert(Comment repository);
    Task<Maybe<Comment>> Update(Comment repository);
    Task<bool> Delete(Guid id);
}