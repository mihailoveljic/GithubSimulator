using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> Delete(Guid userId);
        Task<User> GetById(Guid userId);
        Task<User> Insert(User user);
        Task<Maybe<User>> Update(User updatedUser);
    }
}
