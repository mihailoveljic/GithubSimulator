using CSharpFunctionalExtensions;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> CheckUserPassword(Guid userId, string password);

		Task<Result<string>> Authenticate(string email, string password);
    }
}