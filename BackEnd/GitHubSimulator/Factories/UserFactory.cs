﻿using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Dtos.Users;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

namespace GitHubSimulator.Factories;

public class UserFactory
{
    public User MapToDomain(UserRegistrationDto dto, bool isAdmin)
    {
        var email = Mail.Create(dto.Email);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var accountCredentials = AccountCredentials.Create(dto.Username, passwordHash);

        return User.Create(dto.Name, dto.Surname, email, accountCredentials, isAdmin ? "Administrator" : "RegularUser");
    }

    public User MapToDomain(UpdateUserDto dto, Guid userId, bool isAdmin)
    {
        var email = Mail.Create(dto.Email);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var accountCredentials = AccountCredentials.Create(dto.Username, passwordHash);

        return User.Create(dto.Name, dto.Surname, email, accountCredentials, isAdmin ? "Administrator" : "RegularUser", userId);
    }

    public GetUserDto MapToDto(User user)
    {
        return new GetUserDto(user.Name, user.Surname, user.Mail.Email, user.AccountCredentials.UserName);
    }

    public GiteaUserDto MapToGiteaDto(UserRegistrationDto dto)
    {
        return new GiteaUserDto(dto.Email, dto.Username, dto.Password, false, dto.Name + " " + dto.Surname);
    }
    public List<GetAllUserDto> MapUserListToDtoList(IEnumerable<User> users)
    {
        return users.Select(user => new GetAllUserDto(user.Id, user.Mail)).ToList();
    }
}
