using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.Cache;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface ICacheService
{
    Task<List<Repository>> GetAllRepositoriesAsync();
    Task<bool> SetRepositoryData(Repository value, DateTimeOffset expirationTime);
    Task RemoveAllRepositoryDataAsync();
    Task<List<Issue>> GetAllIssuesAsync();
    Task<bool> SetIssueData(Issue value, DateTimeOffset expirationTime);
    Task RemoveAllIssueDataAsync();
    Task<List<Branch>> GetAllBranchesAsync();
    Task<bool> SetBranchData(Branch value, DateTimeOffset expirationTime);
    Task RemoveAllBranchDataAsync();
    //bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
    //object RemoveData(string key);
    Task<SearchResult> SearchAllIndexesAsync(string searchTerm);
}
