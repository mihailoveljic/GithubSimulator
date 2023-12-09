using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<string>> Authenticate(Mail email, string password);
    }
}