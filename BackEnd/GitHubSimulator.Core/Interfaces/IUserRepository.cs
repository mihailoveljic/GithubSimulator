using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> Delete(Guid userId);
        Task<User> GetById(Guid userId);
        Task<User> GetByEmail(Mail email);
        Task<User> Insert(User user);
        Task<Maybe<User>> Update(User updatedUser);
    }
}
