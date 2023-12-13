﻿namespace GitHubSimulator.Dtos.Users
{
    public record UpdateUserDto(
               Guid Id,
               string Name,
               string Surname,
               string Email,
               string Username,
               string Password);
}
