using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.Cache;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface ICacheService
{
    Task<List<Repository>> GetAllRepositoriesAsync();
    Task<bool> SetRepositoryData(Repository value, DateTimeOffset expirationTime);
    Task RemoveAllRepositoryDataAsync();
    bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
    object RemoveData(string key);
    Task<SearchResult> SearchAllIndexesAsync(string searchTerm);
}
