using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> Delete(Guid userId);
        Task<User> GetById(Guid userId);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<User> Insert(User user);
        Task<Maybe<User>> Update(User updatedUser);
        Task<bool> UpdatePassword(Guid userId, string newPassword);

        Task<IEnumerable<User>> GetUsersNotInRepository(string repoName, string searchString);
    }
}
