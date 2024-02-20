namespace GitHubSimulator.Dtos.Users
{
    public record UpdateUserDto(
               string Name,
               string Surname,
               string Email,
               string Username,
               string Password);
}
