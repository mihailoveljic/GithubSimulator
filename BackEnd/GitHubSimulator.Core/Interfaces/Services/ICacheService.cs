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
    Task<List<Milestone>> GetAllMilestonesAsync();
    Task<bool> SetMilestoneData(Milestone value, DateTimeOffset expirationTime);
    Task RemoveAllMilestoneDataAsync();
    Task<SearchResult> SearchAllIndexesAsync(string searchTerm);
}
