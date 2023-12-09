using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models.AggregateRoots;

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

    public Task<User> GetById(Guid userId) =>
        _userReposityory.GetById(userId);

    public Task<User> Insert(User user) =>
        _userReposityory.Insert(user);

    public Task<Maybe<User>> Update(User user) =>
        _userReposityory.Update(user);
}
