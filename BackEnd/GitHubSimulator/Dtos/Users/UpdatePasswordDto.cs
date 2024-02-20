namespace GitHubSimulator.Dtos.Users
{
    public record UpdatePasswordDto(
               string CurrentPassword,
               string NewPassword);
}
