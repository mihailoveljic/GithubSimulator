namespace GitHubSimulator.Infrastructure.RemoteRepository.Dtos
{
    public record GiteaUserDto(
                string email,
                string username,
                string password,
                bool must_change_password,
                string full_name);
}
