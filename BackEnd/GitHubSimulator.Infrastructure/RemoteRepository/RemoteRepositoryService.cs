using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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

        public async Task CreateRepository(string username, GiteaRepositoryDto repositoryDto)
        {
            var response = await _httpClient.PostAsync($"admin/users/{username}/repos", JsonContent.Create(repositoryDto));
            response.EnsureSuccessStatusCode();
        }
    }
}
