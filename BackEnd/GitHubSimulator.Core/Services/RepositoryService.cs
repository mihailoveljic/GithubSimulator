using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
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

    public async Task<bool> Delete(Guid id)
    {
        return await _repositoryRepository.Delete(id);
    }
}