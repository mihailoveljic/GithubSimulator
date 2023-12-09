using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> Delete(Guid id);
        Task<User> GetById(Guid userId);
        Task<User> GetByEmail(Mail mail);
        Task<User> Insert(User user);
        Task<Maybe<User>> Update(User user);
    }
}