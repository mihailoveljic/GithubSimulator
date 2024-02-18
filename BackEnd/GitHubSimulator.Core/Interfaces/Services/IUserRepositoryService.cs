using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IUserRepositoryService
{
    Task<IEnumerable<UserRepository>> GetAll();
    Task<IEnumerable<UserRepository>> GetByUserName(string userName);
    Task<IEnumerable<UserRepository>> GetByRepositoryName(string repoName);
    Task<UserRepository> AddUserToRepository(UserRepository repository);
    Task<bool> RemoveUserFromRepository(string userName, string repoName);
    Task<Maybe<UserRepository>> ChangeUserRole(string userName, string repoName, UserRepositoryRole newRole);
    Task<Maybe<UserRepository>> UpdateRepositoryName(string repositoryName, string newName);
}