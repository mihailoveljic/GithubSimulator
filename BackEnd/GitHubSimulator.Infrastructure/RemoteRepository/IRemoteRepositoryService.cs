using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

namespace GitHubSimulator.Infrastructure.RemoteRepository
{
    public interface IRemoteRepositoryService
    {
        Task CreateUser(GiteaUserDto userDto);
        Task CreateRepository(string username, CreateGiteaRepositoryDto repositoryDto);
        Task<IEnumerable<GetGiteaRepositoryDto>> GetUserRepositories(string username, int page, int limit);
        Task<IEnumerable<GiteaDocumentDto>> GetRepositoryContent(string owner, string repositoryName, string filePath, string branchName);
        Task<GetGiteaRepositoryDto> GetRepository(string username, string repoName);
        Task ForkRepo(string username, string owner, string repoName, string forkName);
        Task WatchRepo(string username, string owner, string repoName);
        Task UnwatchRepo(string username, string owner, string repoName);
        Task<bool> IsUserWatchingRepo(string username, string owner, string repoName);
        Task StarRepo(string username, string owner, string repoName);
        Task UnstarRepo(string username, string owner, string repoName);
        Task<bool> IsUserStarredRepo(string username, string owner, string repoName);
    }
}