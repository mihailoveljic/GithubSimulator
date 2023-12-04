using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;

namespace GitHubSimulator.Core.Interfaces;

public interface IRepositoryRepository
{
    Task<Repository> GetById(Guid id);
    Task<IEnumerable<Repository>> GetAll();
    Task<Repository> Insert(Repository repository);
    Task<Maybe<Repository>> Update(Repository repository);
    Task<bool> Delete(Guid id);
}