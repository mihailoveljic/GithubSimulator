using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> Delete(Guid id);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid userId);
        Task<User> GetByEmail(string mail);
        Task<User> GetByUsername(string username);
        Task<User> Insert(User user);
        Task<Maybe<User>> Update(User user);
        Task<bool> UpdatePassword(Guid userId, string newPassword);
    }
}