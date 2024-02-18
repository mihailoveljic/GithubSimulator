using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.AggregateRoots;

namespace GitHubSimulator.Core.Services;

public class RepositoryService : IRepositoryService
{
    private readonly IRepositoryRepository _repositoryRepository;

    public RepositoryService(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Repository> GetById(Guid id)
    {
        return await _repositoryRepository.GetById(id);
    }

    public async Task<IEnumerable<Repository>> GetAll()
    {
        return await _repositoryRepository.GetAll();
    }

    public async Task<Repository> Insert(Repository repository)
    {
        return await _repositoryRepository.Insert(repository);
    }

    public async Task<Maybe<Repository>> Update(Repository repository)
    {
        return await _repositoryRepository.Update(repository);
    }

    public async Task<bool> Delete(string name)
    {
        return await _repositoryRepository.Delete(name);
    }

    public async Task<Maybe<Repository>> UpdateName(string repositoryName, string newName)
    {
        return await _repositoryRepository.UpdateName(repositoryName, newName);
    }

    public async Task<Maybe<Repository>> UpdateVisibility(string repositoryName, bool isPrivate)
    {
        return await _repositoryRepository.UpdateVisibility(repositoryName, isPrivate);    
    }

    public async Task<Maybe<Repository>> UpdateRepositoryOwner(string repositoryName, string newOwner)
    {
        return await _repositoryRepository.UpdateRepositoryOwner(repositoryName, newOwner);
    }

    public async Task<string> GetRepositoryOwner(string repo)
    {
        return await _repositoryRepository.GetRepositoryOwner(repo);
    }
}