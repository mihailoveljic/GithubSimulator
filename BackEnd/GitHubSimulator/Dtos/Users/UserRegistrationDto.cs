namespace GitHubSimulator.Dtos.Users
{
    public record UserRegistrationDto(
               string Name,
               string Surname,
               string Email,
               string Username,
               string Password);
}
