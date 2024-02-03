﻿using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Cache;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.Factories;
using GitHubSimulator.Infrastructure.Models;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Globalization;
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
            Branches = (await SearchForBranchesAsync(searchTerm)).Select(BranchFactory.MapToDomain).ToList(),
            Comments = (await SearchForCommentsAsync(searchTerm)).Select(CommentFactory.MapToDomain).ToList(),
            Issues = (await SearchForIssuesAsync(searchTerm)).Select(IssueFactory.MapToDomain).ToList(),
            Labels = (await SearchForLabelsAsync(searchTerm)).Select(LabelFactory.MapToDomain).ToList(),
            Milestones = (await SearchForMilestonesAsync(searchTerm)).Select(MilestoneFactory.MapToDomain).ToList()
        };
        return searchResult;
    }

    private async Task<List<Repository>> SearchForRepositoriesAsync(string searchTerm, int limit = 100)
    {
        var results = new List<Models.Repository>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Repository", $"@Name:{searchTerm} | @Description:{searchTerm}", "LIMIT", "0", limit.ToString());

        if (redisResult.IsNull) return results;

        var redisArray = (RedisResult[])redisResult;
        // Iterate through results, skipping the first element (total count)
        for (int i = 1; i < redisArray.Length; i += 2)
        {
            // The key is at index i, and the field-value pairs are at index i + 1
            var key = redisArray[i].ToString();
            var resultData = (RedisResult[])redisArray[i + 1];
            var repository = new Models.Repository();

            // Parse the Id from the key
            var idStr = key.Split(':')[1];
            repository.Id = Guid.Parse(idStr);

            for (int j = 0; j < resultData.Length; j += 2)
            {
                var field = resultData[j].ToString();
                var value = resultData[j + 1].ToString();

                switch (field)
                {
                    case "Name":
                        repository.Name = value;
                        break;
                    case "Description":
                        repository.Description = value;
                        break;
                    case "Visibility":
                        repository.Visibility = Enum.Parse<Visibility>(value);
                        break;
                        // Note: No need to parse Id here as it's obtained from the key
                }
            }
            results.Add(repository);
        }

        return results;
    }
    private async Task<List<Branch>> SearchForBranchesAsync(string searchTerm, int limit = 100)
    {
        var results = new List<Models.Branch>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Branch", $"@Name:{searchTerm}", "LIMIT", "0", limit.ToString());

        if (redisResult.IsNull) return results;

        var redisArray = (RedisResult[])redisResult;
        // Iterate through results, skipping the first element (total count)
        for (int i = 1; i < redisArray.Length; i += 2)
        {
            // The key is at index i, and the field-value pairs are at index i + 1
            var key = redisArray[i].ToString();
            var resultData = (RedisResult[])redisArray[i + 1];
            var branch = new Models.Branch();

            // Parse the Id from the key
            var idStr = key.Split(':')[1];
            branch.Id = Guid.Parse(idStr);

            for (int j = 0; j < resultData.Length; j += 2)
            {
                var field = resultData[j].ToString();
                var value = resultData[j + 1].ToString();

                switch (field)
                {
                    case "Name":
                        branch.Name = value;
                        break;
                    case "RepositoryId":
                        branch.RepositoryId = Guid.Parse(value);
                        break;
                    case "IssueId":
                        branch.IssueId = Guid.Parse(value);
                        break;
                }
            }
            results.Add(branch);
        }

        return results;
    }
    private async Task<List<Comment>> SearchForCommentsAsync(string searchTerm, int limit = 100)
    {
        var results = new List<Models.Comment>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Comment", $"@Content:{searchTerm}", "LIMIT", "0", limit.ToString());

        if (redisResult.IsNull) return results;

        var redisArray = (RedisResult[])redisResult;
        // Iterate through results, skipping the first element (total count)
        for (int i = 1; i < redisArray.Length; i += 2)
        {
            // The key is at index i, and the field-value pairs are at index i + 1
            var key = redisArray[i].ToString();
            var resultData = (RedisResult[])redisArray[i + 1];
            var comment = new Models.Comment();

            // Parse the Id from the key
            var idStr = key.Split(':')[1];
            comment.Id = Guid.Parse(idStr);

            for (int j = 0; j < resultData.Length; j += 2)
            {
                var field = resultData[j].ToString();
                var value = resultData[j + 1].ToString();

                switch (field)
                {
                    case "Content":
                        comment.Content = value;
                        break;
                    case "TaskId":
                        comment.TaskId = Guid.Parse(value);
                        break;
                }
            }
            results.Add(comment);
        }

        return results;
    }
    private async Task<List<Issue>> SearchForIssuesAsync(string searchTerm, int limit = 100)
    {
        var results = new List<Models.Issue>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Issue", $"@Title:{searchTerm} | @Description:{searchTerm} | @AssigneeEmail:{searchTerm}", "LIMIT", "0", limit.ToString());

        if (redisResult.IsNull) return results;

        var redisArray = (RedisResult[])redisResult;
        // Iterate through results, skipping the first element (total count)
        for (int i = 1; i < redisArray.Length; i += 2)
        {
            // The key is at index i, and the field-value pairs are at index i + 1
            var key = redisArray[i].ToString();
            var resultData = (RedisResult[])redisArray[i + 1];
            var issue = new Models.Issue();

            // Parse the Id from the key
            var idStr = key.Split(':')[1];
            issue.Id = Guid.Parse(idStr);
            for (int j = 0; j < resultData.Length; j += 2)
            {
                var field = resultData[j].ToString();
                var value = resultData[j + 1].ToString();

                switch (field)
                {
                    case "Title":
                        issue.Title = value;
                        break;
                    case "Description":
                        issue.Description = value;
                        break;
                    case "CreatedAt":
                        issue.CreatedAt = ParseDateString(value);
                        break;
                    case "AssigneeEmail":
                        issue.AssigneeEmail = value;
                        break;
                    case "RepositoryId":
                        issue.RepositoryId = Guid.Parse(value);
                        break;
                    case "MilestoneId":
                        issue.MilestoneId = value is not null ? Guid.Parse(value) : null;
                        break;
                }
            }
            results.Add(issue);
        }

        return results;
    }
    private async Task<List<Label>> SearchForLabelsAsync(string searchTerm, int limit = 100)
    {
        var results = new List<Models.Label>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Label", $"@Name:{searchTerm} | @Description:{searchTerm}", "LIMIT", "0", limit.ToString());

        if (redisResult.IsNull) return results;

        var redisArray = (RedisResult[])redisResult;
        // Iterate through results, skipping the first element (total count)
        for (int i = 1; i < redisArray.Length; i += 2)
        {
            // The key is at index i, and the field-value pairs are at index i + 1
            var key = redisArray[i].ToString();
            var resultData = (RedisResult[])redisArray[i + 1];
            var label = new Models.Label();

            // Parse the Id from the key
            var idStr = key.Split(':')[1];
            label.Id = Guid.Parse(idStr);
            for (int j = 0; j < resultData.Length; j += 2)
            {
                var field = resultData[j].ToString();
                var value = resultData[j + 1].ToString();

                switch (field)
                {
                    case "Name":
                        label.Name = value;
                        break;
                    case "Description":
                        label.Description = value;
                        break;
                    case "Color":
                        label.Color = value;
                        break;
                }
            }
            results.Add(label);
        }

        return results;
    }
    private async Task<List<Milestone>> SearchForMilestonesAsync(string searchTerm, int limit = 100)
    {
        var results = new List<Models.Milestone>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Milestone", $"@Title:{searchTerm} | @Description:{searchTerm}", "LIMIT", "0", limit.ToString());

        if (redisResult.IsNull) return results;

        var redisArray = (RedisResult[])redisResult;
        // Iterate through results, skipping the first element (total count)
        for (int i = 1; i < redisArray.Length; i += 2)
        {
            // The key is at index i, and the field-value pairs are at index i + 1
            var key = redisArray[i].ToString();
            var resultData = (RedisResult[])redisArray[i + 1];
            var milestone = new Models.Milestone();
            var idStr = key.Split(':')[1];
            milestone.Id = Guid.Parse(idStr);
            for (int j = 0; j < resultData.Length; j += 2)
            {
                var field = resultData[j].ToString();
                var value = resultData[j + 1].ToString();

                switch (field)
                {
                    case "Title":
                        milestone.Title = value;
                        break;
                    case "Description":
                        milestone.Description = value;
                        break;
                    case "DueDate":
                        milestone.DueDate = ParseDateString(value);
                        break;
                    case "State":
                        milestone.State = Enum.Parse<State>(value);
                        break;
                    case "RepositoryId":
                        milestone.RepositoryId = Guid.Parse(value);
                        break;
                }
            }
            results.Add(milestone);
        }

        return results;
    }
    private DateTime? ParseDateString(string dateString)
    {
        string format = "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz";

        string parseablePart = dateString.Split('(')[0].Trim();

        if (DateTimeOffset.TryParseExact(parseablePart, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset dateTimeOffset))
        {
            return dateTimeOffset.DateTime;
        }
        else
        {
            if (DateTime.TryParse(dateString, out DateTime dateTime))
            {
                return dateTime;
            }
        }
        return null;
    }

}
