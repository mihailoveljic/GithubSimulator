using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Dtos.Users;

public record GetAllUserDto(
    Guid Id,
    Mail Email
    );