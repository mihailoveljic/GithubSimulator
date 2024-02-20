using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

namespace GitHubSimulator.Infrastructure.RemoteRepository
{
    public interface IRemoteRepositoryService
    {
        Task CreateUser(GiteaUserDto userDto);
        Task CreateRepository(string username, CreateGiteaRepositoryDto repositoryDto);
        Task<IEnumerable<GetGiteaRepositoryDto>> GetUserRepositories(string username, int page, int limit);
        Task<GetGiteaRepositoryDto> GetRepositoryByName(string owner, string repo);
        Task<GetGiteaRepositoryDto> UpdateRepositoryName(string owner, string repo, string name);
        Task<GetGiteaRepositoryDto> UpdateRepositoryVisibility(string owner, string repo, bool isPrivate);
        Task<GetGiteaRepositoryDto> UpdateRepositoryArchivedState(string owner, string repo, bool isArchived);
        Task<GetGiteaRepositoryDto> UpdateRepositoryOwner(string owner, string repo, string newOwner);
        Task AddCollaboratorToRepository(string owner, string repo, string collaborator);
        Task RemoveCollaboratorFromRepository(string owner, string repo, string collaborator);
        Task DeleteRepository(string owner, string repo);
        Task<IEnumerable<GiteaDocumentDto>> GetRepositoryContent(string owner, string repositoryName, string filePath, string branchName);
        Task<GetGiteaRepositoryDto> GetRepository(string username, string repoName);
        Task ForkRepo(string username, string owner, string repoName, string forkName);
        Task WatchRepo(string username, string owner, string repoName);
        Task UnwatchRepo(string username, string owner, string repoName);
        Task<bool> IsUserWatchingRepo(string username, string owner, string repoName);
        Task StarRepo(string username, string owner, string repoName);
        Task UnstarRepo(string username, string owner, string repoName);
        Task<bool> IsUserStarredRepo(string username, string owner, string repoName);
        Task<IEnumerable<GetGiteaBranchResponseDto>> GetRepositoryBranches(string owner, string repositoryName);
        Task<GetGiteaRepositoryDto> UpdateRepositoryDefaultBranch(string owner, string repo, string defaultBranch);
    }
}