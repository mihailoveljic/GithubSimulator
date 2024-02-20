namespace GitHubSimulator.Dtos.Users
{
    public record GetUserDto(
               string Name,
               string Surname,
               string Email,
               string Username);
}
