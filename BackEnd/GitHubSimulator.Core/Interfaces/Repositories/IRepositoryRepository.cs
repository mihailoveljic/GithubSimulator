using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;

namespace GitHubSimulator.Core.Interfaces.Repositories;

public interface IRepositoryRepository
{
    Task<Repository> GetById(Guid id);
    Task<IEnumerable<Repository>> GetAll(); 
    Task<Repository> Insert(Repository repository);
    Task<Maybe<Repository>> Update(Repository repository);
    Task<bool> Delete(string name);
    Task<Maybe<Repository>> UpdateName(string repositoryName, string newName);
    Task<Maybe<Repository>> UpdateVisibility(string repositoryName, bool isPrivate);
    Task<Maybe<Repository>> UpdateRepositoryOwner(string repositoryName, string newOwner);
    Task<string> GetRepositoryOwner(string repo);
    Task<Repository> GetByName(string name);
}