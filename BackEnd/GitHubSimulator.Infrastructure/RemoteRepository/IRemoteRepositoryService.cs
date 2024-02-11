using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

namespace GitHubSimulator.Infrastructure.RemoteRepository
{
    public interface IRemoteRepositoryService
    {
        Task CreateUser(GiteaUserDto userDto);
    }
}