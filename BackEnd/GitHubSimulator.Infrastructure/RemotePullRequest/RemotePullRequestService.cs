using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.RemotePullRequest.Dtos;
using GitHubSimulator.Infrastructure.RemoteRepository;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace GitHubSimulator.Infrastructure.RemotePullRequest
{
    public sealed class RemotePullRequestService : IRemotePullRequestService
    {
        private readonly HttpClient _httpClient;

        public RemotePullRequestService(IOptions<RemoteRepositorySettings> remoteRepositorySettings, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", remoteRepositorySettings.Value.AdminAccessToken);
            _httpClient.BaseAddress = new Uri(remoteRepositorySettings.Value.BaseURL);
        }

        public async Task<GiteaPullRequestDto> CreatePullRequest(string username, string repo, CreateGiteaPullRequest pullRequestDto)
        {
            var response = await _httpClient.PostAsync($"repos/{username}/{repo}/pulls", JsonContent.Create(pullRequestDto));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GiteaPullRequestDto>();
        }

        public async Task<GiteaPullRequestDto> GetPullRequest(string username, string repo, string index)
        {
            var response = await _httpClient.GetAsync($"repos/{username}/{repo}/pulls/{index}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GiteaPullRequestDto>();
        }

        public async Task<IEnumerable<GiteaCommitDto>> GetPullRequestCommits(string username, string repo, string index)
        {
            var response = await _httpClient.GetAsync($"repos/{username}/{repo}/pulls/{index}/commits");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<GiteaCommitDto>>();
        }

        public async Task<string> GetPullRequestDiff(string username, string repo, string index)
        {
            var response = await _httpClient.GetAsync($"repos/{username}/{repo}/pulls/{index}.diff");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CommitDiff(string username, string repo, string sha)
        {
            var response = await _httpClient.GetAsync($"repos/{username}/{repo}/git/commits/{sha}.diff");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task MergePullRequest(string username, string repo, string index, MergeGiteaPullRequest pullRequestDto)
        {
            var response = await _httpClient.PostAsync($"repos/{username}/{repo}/pulls/{index}/merge",
                JsonContent.Create(pullRequestDto));

            response.EnsureSuccessStatusCode();
        }
    }
}
