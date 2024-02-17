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

        public async Task<IEnumerable<GiteaDocumentDto>> GetRepositoryContent(string owner, string repositoryName, string filePath, string branchName)
        {
            var response = await _httpClient.GetAsync($"repos/{owner}/{repositoryName}/contents/{filePath}?ref={branchName}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<GiteaDocumentDto>>();
        }
    }
}
