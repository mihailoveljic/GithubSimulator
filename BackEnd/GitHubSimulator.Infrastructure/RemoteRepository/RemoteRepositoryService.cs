using System.Net;
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

        public async Task<GetGiteaRepositoryDto> GetRepository(string username, string repoName)
        {
            var response = await _httpClient.GetAsync($"repos/{username}/{repoName}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetGiteaRepositoryDto>();
        }

        public async Task<IEnumerable<GiteaDocumentDto>> GetRepositoryContent(string owner, string repositoryName, string filePath, string branchName)
        {
            var response = await _httpClient.GetAsync($"repos/{owner}/{repositoryName}/contents/{filePath}?ref={branchName}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<GiteaDocumentDto>>();
        }

        public async Task ForkRepo(string username, string owner, string repoName, string forkName)
        {
            var response = await _httpClient.PostAsync($"repos/{owner}/{repoName}/forks?sudo={username}", JsonContent.Create(new { name = forkName }));
            response.EnsureSuccessStatusCode();
        }

        public async Task WatchRepo(string username, string owner, string repoName)
        {
            var response = await _httpClient.PutAsync($"repos/{owner}/{repoName}/subscription?sudo={username}", JsonContent.Create(new { }));
            response.EnsureSuccessStatusCode();
        }

        public async Task UnwatchRepo(string username, string owner, string repoName)
        {
            var response = await _httpClient.DeleteAsync($"repos/{owner}/{repoName}/subscription?sudo={username}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsUserWatchingRepo(string username, string owner, string repoName)
        {
            var response = await _httpClient.GetAsync($"repos/{owner}/{repoName}/subscription?sudo={username}");
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task StarRepo(string username, string owner, string repoName)
        {
            var response = await _httpClient.PutAsync($"user/starred/{owner}/{repoName}?sudo={username}", JsonContent.Create( new { }));
            response.EnsureSuccessStatusCode();
        }

        public async Task UnstarRepo(string username, string owner, string repoName)
        {
            var response = await _httpClient.DeleteAsync($"user/starred/{owner}/{repoName}?sudo={username}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsUserStarredRepo(string username, string owner, string repoName)
        {
            var response = await _httpClient.GetAsync($"user/starred/{owner}/{repoName}?sudo={username}");
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }
    }
}
