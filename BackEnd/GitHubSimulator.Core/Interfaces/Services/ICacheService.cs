using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.Cache;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface ICacheService
{
    Task<List<Repository>> GetAllRepositoriesAsync(string userName);
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
    Task<List<Comment>> GetAllCommentsAsync();
    Task<bool> SetCommentData(Comment value, DateTimeOffset expirationTime);
    Task RemoveAllCommentDataAsync();
    Task<List<Label>> GetAllLabelsAsync();
    Task<bool> SetLabelData(Label value, DateTimeOffset expirationTime);
    Task RemoveAllLabelDataAsync();
    Task<SearchResult> SearchAllIndexesAsync(string searchTerm);
}
