using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

namespace GitHubSimulator.Infrastructure.RemoteRepository
{
    public interface IRemoteRepositoryService
    {
        Task CreateUser(GiteaUserDto userDto);
        Task CreateRepository(string username, CreateGiteaRepositoryDto repositoryDto);
        Task<IEnumerable<GetGiteaRepositoryDto>> GetUserRepositories(string username, int page, int limit);

    }
}