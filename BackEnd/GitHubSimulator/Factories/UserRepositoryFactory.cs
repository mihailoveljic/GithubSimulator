using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Dtos.UserRepositories;

namespace GitHubSimulator.Factories;

public class UserRepositoryFactory
{
    public UserRepository MapToDomain(string userName, string repoName,
        UserRepositoryRole userRepositoryRole) =>
        UserRepository.Create(
            userName,
            repoName,
            userRepositoryRole
        );

    public GetUserRepositoriesByRepositoryNameAltResultDto MapToDto(
        UserRepository userRepository, string userEmail)
    {
        return new GetUserRepositoriesByRepositoryNameAltResultDto(userRepository.Id, userRepository.RepositoryName,
            userEmail, userRepository.UserRepositoryRole);
    }
}