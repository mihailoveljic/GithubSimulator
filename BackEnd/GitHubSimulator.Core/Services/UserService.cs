using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userReposityory;

    public UserService(IUserRepository _userReposityory)
    {
        this._userReposityory = _userReposityory;
    }

    public Task<bool> Delete(Guid id) =>
        _userReposityory.Delete(id);

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userReposityory.GetAll();
    }

    public Task<User> GetById(Guid userId) =>
        _userReposityory.GetById(userId);

    public Task<User> GetByEmail(string mail) =>
        _userReposityory.GetByEmail(mail);
    public Task<User> GetByUsername(string username) =>
    _userReposityory.GetByUsername(username);

    public Task<User> Insert(User user) =>
        _userReposityory.Insert(user);

    public Task<bool> UpdatePassword(Guid userId, string newPassword) =>
        _userReposityory.UpdatePassword(userId, newPassword);

    public async Task<IEnumerable<User>> GetUsersNotInRepository(string repoName, string searchString)
    {
        return await _userReposityory.GetUsersNotInRepository(repoName, searchString);
    }

    public Task<Maybe<User>> Update(User user) =>
        _userReposityory.Update(user);
}
