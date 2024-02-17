using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.RemoteBranch.Dtos;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemoteBranch
{

    public sealed class RemoteBranchService : IRemoteBranchService
    {

        private readonly HttpClient _httpClient;

        public RemoteBranchService(IOptions<RemoteRepositorySettings> remoteRepositorySettings)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", remoteRepositorySettings.Value.AdminAccessToken);
            _httpClient.BaseAddress = new Uri(remoteRepositorySettings.Value.BaseURL);
        }


        public async Task CreateBranch(string username, string repo, CreateGiteaBranchDto branchDto)
        {
            var response = await _httpClient.PostAsync($"repos/{username}/{repo}/branches", JsonContent.Create(branchDto));
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBranch(string username, string repo, string branch)
        {
            var response = await _httpClient.DeleteAsync($"repos/{username}/{repo}/branches/{branch}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<GetGiteaBranchesDto> GetBranch(string username, string repo, string branch)
        {
            var response = await _httpClient.GetAsync($"repos/{username}/{repo}/branches/{branch}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetGiteaBranchesDto>();
        }

        public async Task<IEnumerable<GetGiteaBranchesDto>> GetRepoBranches(string username, string repo, int page, int limit)
        {
            var response = await _httpClient.GetAsync($"repos/{username}/{repo}/branches?page={page}&limit={limit}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<GetGiteaBranchesDto>>();
        }

        public async Task<GetGiteaBranchesDto> UpdateBranch(string username, string repo, GetGiteaBranchesDto dto)
        {
            var response = await _httpClient.PutAsync($"repos/{username}/{repo}/branches", JsonContent.Create(dto));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<GetGiteaBranchesDto>();
        }
    }
}
