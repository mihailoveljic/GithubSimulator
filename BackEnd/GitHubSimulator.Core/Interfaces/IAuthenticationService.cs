using CSharpFunctionalExtensions;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<string>> Authenticate(string email, string password);
    }
}