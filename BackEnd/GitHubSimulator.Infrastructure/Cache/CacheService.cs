using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Cache;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.Factories;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace GitHubSimulator.Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;
    private readonly Dictionary<string, string[]> _indexFields = new Dictionary<string, string[]>
    {
        { "idx:Branch", new[] { "Name" } },
        { "idx:Comment", new[] { "Content" } },
        { "idx:Issue", new[] { "Title", "Description", "AssigneeEmail" } },
        { "idx:Label", new[] { "Name", "Description" } },
        { "idx:Milestone", new[] { "Title", "Description" } },
        { "idx:Repository", new[] { "Name", "Description" } },
        { "idx:User", new[] { "Name", "Surname", "Role" } }
    };


    public CacheService(IOptions<RedisSettings> redisSettings)
    {
        var configurationOptions = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            EndPoints = { $"{redisSettings.Value.URL}" },
            Password = redisSettings.Value.Password
        };
        var redis = ConnectionMultiplexer.Connect(configurationOptions);
        _cacheDb = redis.GetDatabase();
    }
    public T GetData<T>(string key)
    {
        var value = _cacheDb.StringGet(key);
        if(!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        return default;
    }

    public object RemoveData(string key)
    {
        var exist = _cacheDb.KeyExists(key);

        if(exist)
        {
            return _cacheDb.KeyDelete(key);
        }
        return false;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expTime);
    }
    public async Task<SearchResult> SearchAllIndexesAsync(string searchTerm)
    {
        var searchResult = new SearchResult
        {
            Repositories = (await SearchForRepositoriesAsync(searchTerm)).Select(RepositoryFactory.MapToDomain).ToList(),
            //Branches = await SearchForBranches(searchTerm),
            //Comments = await SearchForComments(searchTerm),
            //Issues = await SearchForIssues(searchTerm),
            //Labels = await SearchForLabels(searchTerm),
            //Milestones = await SearchForMilestones(searchTerm),
            //Users = await SearchForUsers(searchTerm)
        };
        return searchResult;
        //var indexes = new List<string> { "idx:Branch", "idx:Comment", "idx:Issue", "idx:Label", "idx:Milestone", "idx:Repository", "idx:User" };
        //var allResults = new List<SearchResult>();

        //foreach (var index in indexes)
        //{
        //    var results = await SearchAsync(index, searchTerm);
        //    allResults.AddRange(results);
        //}

        //return allResults;
    }

    //private async Task<IEnumerable<SearchResult>> SearchAsync(string index, string searchTerm, int limit = 100)
    //{
    //    var results = new List<SearchResult>();

    //    // ... existing query construction logic

    //    var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", index, query, "LIMIT", "0", limit.ToString());

    //    if (redisResult.IsNull) return results;

    //    var redisArray = (RedisResult[])redisResult;
    //    for (int i = 1; i < redisArray.Length; i++) // Skip the count
    //    {
    //        var resultEntries = (RedisResult[])redisArray[i];
    //        var result = new SearchResult();

    //        // Extract entity type and ID from the key
    //        string key = resultEntries[0].ToString();
    //        result.EntityType = key.Split(':')[0]; // Assuming key format is "EntityType:EntityId"
    //        result.EntityId = key.Split(':')[1];

    //        // Process the field-value pairs
    //        for (int j = 1; j < resultEntries.Length; j += 2) // Start at 1 to skip the key
    //        {
    //            var field = resultEntries[j].ToString();
    //            var value = resultEntries[j + 1].ToString();
    //            result.Fields[field] = value;
    //        }

    //        results.Add(result);
    //    }

    //    return results;
    //}

    private async Task<List<Models.Repository>> SearchForRepositoriesAsync(string searchTerm, int limit = 100)
    {
        var results = new List<Models.Repository>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Repository", $"@Name:{searchTerm} | @Description:{searchTerm}", "LIMIT", "0", limit.ToString());

        if (redisResult.IsNull) return results;

        var redisArray = (RedisResult[])redisResult;
        for (int i = 1; i < redisArray.Length; i++) // Skip the count
        {
            if (!(redisArray[i] is RedisResult resultData)) continue;
            Models.Repository repository = new Models.Repository();

            for (int j = 0; j < resultData.Length; j += 2)
            {
                var field = resultData[j].ToString();
                var value = resultData[j + 1].ToString();

                switch (field)
                {
                    case "Id":
                        repository.Id = Guid.Parse(value);
                        break;
                    case "Name":
                        repository.Name = value;
                        break;
                    case "Description":
                        repository.Description = value;
                        break;
                    case "Visibility":
                        repository.Visibility = Enum.Parse<Visibility>(value);
                        break;
                }
            }
            results.Add(repository);
        }

        return results;
    }

}
