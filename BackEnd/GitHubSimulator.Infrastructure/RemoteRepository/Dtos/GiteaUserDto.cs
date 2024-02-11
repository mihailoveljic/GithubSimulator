namespace GitHubSimulator.Infrastructure.RemoteRepository.Dtos
{
    public record GiteaUserDto(
                string Email,
                string Username,
                string Password,
                bool Must_Change_Password,
                string Full_Name);
}
