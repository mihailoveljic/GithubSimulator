using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IRepositoryService
{
    Task<Repository> GetById(Guid id);
    Task<IEnumerable<Repository>> GetAll();
    Task<Repository> Insert(Repository repository);
    Task<Maybe<Repository>> Update(Repository repository);
    Task<bool> Delete(string id);
    Task<Maybe<Repository>> UpdateName(string repositoryName, string newName);
    Task<Maybe<Repository>> UpdateVisibility(string repositoryName, bool isPrivate);
    Task<Maybe<Repository>> UpdateRepositoryOwner(string repositoryName, string newOwner);
    Task<Repository> GetByName(string name);
    Task<string> GetRepositoryOwner(string repo);
    Task<IEnumerable<Repository>> GetPublicRepositories(int page, int limit);
}