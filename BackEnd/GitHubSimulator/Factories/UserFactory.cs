using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Dtos.Users;

namespace GitHubSimulator.Factories;

public class UserFactory
{
    public User MapToDomain(UserRegistrationDto dto)
    {
        var email = Mail.Create(dto.Email);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var accountCredentials = AccountCredentials.Create(dto.Username, passwordHash);

        return User.Create(dto.Name, dto.Surname, email, accountCredentials);
    }

    public User MapToDomain(UpdateUserDto dto)
    {
        var email = Mail.Create(dto.Email);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var accountCredentials = AccountCredentials.Create(dto.Username, passwordHash);

        return User.Create(dto.Name, dto.Surname, email, accountCredentials, dto.Id);
    }
}
