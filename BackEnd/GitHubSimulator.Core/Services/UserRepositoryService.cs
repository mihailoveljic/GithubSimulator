using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Services;

public class UserRepositoryService : IUserRepositoryService
{
    private readonly IUserRepositoryRepository _userRepositoryRepository;

    public UserRepositoryService(IUserRepositoryRepository userRepositoryRepository)
    {
        _userRepositoryRepository = userRepositoryRepository;
    }

    public async Task<IEnumerable<UserRepository>> GetAll()
    {
        return await _userRepositoryRepository.GetAll();
    }

    public async Task<IEnumerable<UserRepository>> GetByUserName(string userName)
    {
        return await _userRepositoryRepository.GetByUserName(userName);
    }

    public async Task<IEnumerable<UserRepository>> GetByRepositoryName(string repoName)
    {
        return await _userRepositoryRepository.GetByRepositoryName(repoName);
    }

    public async Task<UserRepository> AddUserToRepository(UserRepository repository)
    {
        return await _userRepositoryRepository.AddUserToRepository(repository);
    }

    public async Task<bool> RemoveUserFromRepository(string userName, string repoName)
    {
        return await _userRepositoryRepository.RemoveUserFromRepository(userName, repoName);
    }

    public async Task<Maybe<UserRepository>> ChangeUserRole(string userName, string repoName, UserRepositoryRole newRole)
    {
        return await _userRepositoryRepository.ChangeUserRole(userName, repoName, newRole);
    }
}