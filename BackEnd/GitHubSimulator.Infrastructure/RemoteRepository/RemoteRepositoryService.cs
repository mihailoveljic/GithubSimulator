using System.Net.Http.Headers;
using System.Net.Http.Json;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;
using Microsoft.Extensions.Options;

namespace GitHubSimulator.Infrastructure.RemoteRepository
{
    public sealed class RemoteRepositoryService : IRemoteRepositoryService
    {
        private readonly HttpClient _httpClient;

        public RemoteRepositoryService(IOptions<RemoteRepositorySettings> remoteRepositorySettings)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", remoteRepositorySettings.Value.AdminAccessToken);
            _httpClient.BaseAddress = new Uri(remoteRepositorySettings.Value.BaseURL);
        }

        public async Task CreateUser(GiteaUserDto userDto)
        {
            var response = await _httpClient.PostAsync($"admin/users", JsonContent.Create(userDto));
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateRepository(string username, CreateGiteaRepositoryDto repositoryDto)
        {
            var response = await _httpClient.PostAsync($"admin/users/{username}/repos", JsonContent.Create(repositoryDto));
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<GetGiteaRepositoryDto>> GetUserRepositories(string username, int page, int limit)
        {
            var response = await _httpClient.GetAsync($"users/{username}/repos?page={page}&limit={limit}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<GetGiteaRepositoryDto>>();
        }

        public async Task<GetGiteaRepositoryDto> GetRepositoryByName(string owner, string repo)
        {
            var response = await _httpClient.GetAsync($"repos/{owner}/{repo}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetGiteaRepositoryDto>();
        }

        public async Task<GetGiteaRepositoryDto> UpdateRepositoryName(string owner, string repo, string name)
        {
            var response = await _httpClient
                .PatchAsync($"repos/{owner}/{repo}",
                    JsonContent.Create(new UpdateGiteaRepositoryNameDto(name)));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetGiteaRepositoryDto>();
        }

        public async Task<GetGiteaRepositoryDto> UpdateRepositoryVisibility(string owner, string repo, bool isPrivate)
        {
            var response = await _httpClient
                .PatchAsync($"repos/{owner}/{repo}",
                    JsonContent.Create(new UpdateGiteaRepositoryVisibilityDto(isPrivate)));
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<GetGiteaRepositoryDto>();
        }

        public async Task<GetGiteaRepositoryDto> UpdateRepositoryArchivedState(string owner, string repo, bool isArchived)
        {
            var response = await _httpClient
                .PatchAsync($"repos/{owner}/{repo}",
                    JsonContent.Create(new UpdateGiteaRepositoryArchivedStateDto(isArchived)));
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<GetGiteaRepositoryDto>();
        }
        
        public async Task<GetGiteaRepositoryDto> UpdateRepositoryOwner(string owner, string repo, string newOwner)
        {
            var response = await _httpClient
                .PostAsync($"repos/{owner}/{repo}/transfer",
                    JsonContent.Create(new UpdateGiteaRepositoryOwnerDto(newOwner)));
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<GetGiteaRepositoryDto>();
        }

        public async Task AddCollaboratorToRepository(string owner, string repo, string collaborator)
        {
            var response = await _httpClient
                .PutAsync($"repos/{owner}/{repo}/collaborators/{collaborator}",
                    JsonContent.Create(new AddCollaboratorOptionDto("write")));

            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveCollaboratorFromRepository(string owner, string repo, string collaborator)
        {
            var response = await _httpClient
                .DeleteAsync($"repos/{owner}/{repo}/collaborators/{collaborator}");

            response.EnsureSuccessStatusCode();
        }
        
        public async Task DeleteRepository(string owner, string repo)
        {
            var response = await _httpClient
                .DeleteAsync($"repos/{owner}/{repo}");
            response.EnsureSuccessStatusCode();
        }
    }
}
