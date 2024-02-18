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
    }
}