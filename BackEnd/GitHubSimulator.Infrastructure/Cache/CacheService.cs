using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.Cache;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Infrastructure.Configuration;
using GitHubSimulator.Infrastructure.Factories;
using GitHubSimulator.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Globalization;
using System.Text.Json;

namespace GitHubSimulator.Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;

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

    #region searchenginge
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

    private async Task<List<Models.Repository>> SearchForRepositoriesAsync(string searchTerm)
    {
        var results = new List<Models.Repository>();
        var query = $"@Name|Description:{searchTerm}";
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Repository", query);

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
                }
            }
            results.Add(repository);
        }

        return results;
    }
    private async Task<List<Branch>> SearchForBranchesAsync(string searchTerm)
    {
        var results = new List<Branch>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Branch", $"@Name:{searchTerm}");

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
    private async Task<List<Comment>> SearchForCommentsAsync(string searchTerm)
    {
        var results = new List<Comment>();
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Comment", $"@Content:{searchTerm}");

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
    private async Task<List<Issue>> SearchForIssuesAsync(string searchTerm)
    {
        var results = new List<Issue>();
        var query = $"@Title|Description|AssigneeEmail:{searchTerm}";
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Issue", query);

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
    private async Task<List<Label>> SearchForLabelsAsync(string searchTerm)
    {
        var results = new List<Label>();
        var query = $"@Name|Description:{searchTerm}";
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Label", query);

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
    private async Task<List<Milestone>> SearchForMilestonesAsync(string searchTerm)
    {
        var results = new List<Milestone>();
        var query = $"@Title|Description:{searchTerm}";
        var redisResult = await _cacheDb.ExecuteAsync("FT.SEARCH", "idx:Milestone", query);

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
    #endregion

    #region setdata
    private bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _cacheDb.StringSet(key, System.Text.Json.JsonSerializer.Serialize(value), expTime);
    }
    public async Task<bool> SetRepositoryData(Core.Models.AggregateRoots.Repository repository, DateTimeOffset expirationTime)
    {
        try
        {
            var repoKey = $"repository:{repository.Id.ToString("N")}"; // Assuming Id is a Guid in C#

            var hashEntries = new HashEntry[]
            {
            new HashEntry("Id", repository.Id.ToString("N")),
            new HashEntry("Name", repository.Name),
            new HashEntry("Description", repository.Description),
            new HashEntry("Visibility", repository.Visibility.ToString()) // Assuming Visibility is an integer or similar
            };

            await _cacheDb.HashSetAsync(repoKey, hashEntries);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return false;
        }

    }
    public async Task<bool> SetIssueData(Core.Models.Entities.Issue issue, DateTimeOffset expirationTime)
    {
        try
        {
            var issueKey = $"issue:{issue.Id.ToString("N")}"; // Assuming Id is a Guid

            // Convert the CreatedAt DateTime to a suitable string format
            var createdAt = issue.CreatedAt.ToString("o"); // Using the round-trip date/time pattern

            var eventsJson = JsonConvert.SerializeObject(issue.Events);

            var hashEntries = new HashEntry[]
            {
            new HashEntry("Id", issue.Id.ToString("N")),
            new HashEntry("Title", issue.Title),
            new HashEntry("Description", issue.Description),
            new HashEntry("CreatedAt", createdAt),
            new HashEntry("AssigneeEmail", issue.Assignee?.Email),
            new HashEntry("RepositoryId", issue.RepositoryId.ToString("N")),
            new HashEntry("MilestoneId", issue.MilestoneId?.ToString("N")),
            new HashEntry("TaskType", issue.TaskType.ToString()),
            new HashEntry("Events", eventsJson) // Storing serialized events data
            };

            await _cacheDb.HashSetAsync(issueKey, hashEntries);

            // Optionally, set an expiration time for the issue data
            if (expirationTime != DateTimeOffset.MinValue)
            {
                await _cacheDb.KeyExpireAsync(issueKey, expirationTime.UtcDateTime - DateTime.UtcNow);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return false;
        }
    }

    public async Task<bool> SetBranchData(Core.Models.Entities.Branch branch, DateTimeOffset expirationTime)
    {
        try
        {
            var branchKey = $"branch:{branch.Id.ToString("N")}"; // Assuming Id is a Guid

            // Serialize the Commits list to a JSON string
            var commitsJson = JsonConvert.SerializeObject(branch.Commits);

            var hashEntries = new HashEntry[]
            {
            new HashEntry("Id", branch.Id.ToString("N")),
            new HashEntry("Name", branch.Name),
            new HashEntry("RepositoryId", branch.RepositoryId.ToString("N")),
            new HashEntry("IssueId", branch.IssueId?.ToString("N")), // Assuming IssueId is nullable
            new HashEntry("Commits", commitsJson) // Storing serialized commits data
            };

            await _cacheDb.HashSetAsync(branchKey, hashEntries);

            // Optionally, set an expiration time for the branch data
            if (expirationTime != DateTimeOffset.MinValue)
            {
                await _cacheDb.KeyExpireAsync(branchKey, expirationTime.UtcDateTime - DateTime.UtcNow);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return false;
        }
    }

    public bool SetCommentData(Comment comment, DateTimeOffset expirationTime)
    {
        var key = $"comment:{comment.Id}";
        if (!SetData(key, comment, expirationTime)) return false;

        var indexName = "idx:Comment";

        var addCommandArguments = new List<object>
        {
            "FT.ADD", indexName, key, 1.0, "FIELDS",
            "Content", comment.Content
        };

        _cacheDb.Execute("FT.ADD", addCommandArguments.ToArray());

        return true;
    }
    public bool SetLabelData(Label label, DateTimeOffset expirationTime)
    {
        var key = $"label:{label.Id}";
        if (!SetData(key, label, expirationTime)) return false;

        var indexName = "idx:Label";

        var addCommandArguments = new List<object>
        {
            "FT.ADD", indexName, key, 1.0, "FIELDS",
            "Name", label.Name,
            "Description", label.Description
        };

        _cacheDb.Execute("FT.ADD", addCommandArguments.ToArray());

        return true;
    }
    public bool SetMilestoneData(Milestone milestone, DateTimeOffset expirationTime)
    {
        var key = $"milestone:{milestone.Id}";
        if (!SetData(key, milestone, expirationTime)) return false;

        var indexName = "idx:Milestone";

        var addCommandArguments = new List<object>
        {
            "FT.ADD", indexName, key, 1.0, "FIELDS",
            "Title", milestone.Title,
            "Description", milestone.Description
        };

        _cacheDb.Execute("FT.ADD", addCommandArguments.ToArray());

        return true;
    }
    public bool SetUserData(User user, DateTimeOffset expirationTime)
    {
        var key = $"user:{user.Id}";
        if (!SetData(key, user, expirationTime)) return false;

        var indexName = "idx:User";

        var addCommandArguments = new List<object>
        {
            "FT.ADD", indexName, key, 1.0, "FIELDS",
            "Name", user.Name,
            "Surname", user.Surname,
            "Role", user.Role
        };

        _cacheDb.Execute("FT.ADD", addCommandArguments.ToArray());

        return true;
    }
    #endregion

    #region getdata
    public async Task<List<Core.Models.AggregateRoots.Repository>> GetAllRepositoriesAsync()
    {
        var repositories = new List<Core.Models.AggregateRoots.Repository>();
        var indexName = "idx:Repository";

        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return repositories;

        var resultsArray = (RedisResult[])searchResults;
        for (int i = 1; i < resultsArray.Length; i++) // Skipping total count, directly accessing keys
        {
            var key = (string)resultsArray[i];
            var hashEntries = await _cacheDb.HashGetAllAsync(key);
            if (hashEntries.Length > 0)
            {
                // Temporary variables to hold hash values
                Guid id = Guid.Empty;
                string name = null, description = null;
                Visibility visibility = 0;

                // Extract values from hash entries
                foreach (var entry in hashEntries)
                {
                    switch (entry.Name.ToString())
                    {
                        case "Id":
                            id = Guid.Parse(entry.Value);
                            break;
                        case "Name":
                            name = entry.Value;
                            break;
                        case "Description":
                            description = entry.Value;
                            break;
                        case "Visibility":
                            visibility = Enum.Parse<Visibility>(entry.Value);
                            break;
                    }
                }

                // Use the Create method to instantiate a Repository object
                if (id != Guid.Empty && name != null && description != null)
                {
                    var repository = Core.Models.AggregateRoots.Repository.Create(name, description, visibility, id);
                    repositories.Add(repository);
                }
            }
        }

        return repositories;
    }
    public async Task<List<Core.Models.Entities.Issue>> GetAllIssuesAsync()
    {
        var issues = new List<Core.Models.Entities.Issue>();
        var indexName = "idx:Issue";

        // Execute a search query to get all issue keys. The query "*" matches all documents in the index.
        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");

        if (searchResults.IsNull) return issues;

        // Parse the search results
        var resultsArray = (RedisResult[])searchResults;
        for (int i = 1; i < resultsArray.Length; i++) // Skipping the total count, directly accessing keys
        {
            var key = (string)resultsArray[i];
            var hashEntries = await _cacheDb.HashGetAllAsync(key);
            if (hashEntries.Length > 0)
            {
                // Temporary variables to hold hash values
                Guid id = Guid.Empty;
                string title = null, description = null, assigneeEmail = null, repositoryId = null, milestoneId = null;
                DateTime createdAt = DateTime.MinValue;

                // Extract values from hash entries
                foreach (var entry in hashEntries)
                {
                    switch (entry.Name.ToString())
                    {
                        case "Id":
                            id = Guid.Parse(entry.Value);
                            break;
                        case "Title":
                            title = entry.Value;
                            break;
                        case "Description":
                            description = entry.Value;
                            break;
                        case "CreatedAt":
                            createdAt = DateTime.Parse(entry.Value);
                            break;
                        case "AssigneeEmail":
                            assigneeEmail = entry.Value;
                            break;
                        case "RepositoryId":
                            repositoryId = entry.Value;
                            break;
                        case "MilestoneId":
                            milestoneId = entry.Value;
                            break;
                    }
                }

                // Assuming you have a similar Create method or constructor for Issue
                if (id != Guid.Empty && title != null)
                {
                    var issue = Core.Models.Entities.Issue.Create(
                        title,
                        description,
                        Core.Models.ValueObjects.Mail.Create(assigneeEmail),
                        Guid.Parse(repositoryId),
                        Guid.Parse(milestoneId),
                        null,
                        id);
                    issues.Add(issue);
                }
            }
        }

        return issues;
    }

    public async Task<List<Core.Models.Entities.Branch>> GetAllBranchesAsync()
    {
        var branches = new List<Core.Models.Entities.Branch>();
        var indexName = "idx:Branch";

        // Execute a search query to get all branch keys. The query "*" matches all documents in the index.
        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");

        if (searchResults.IsNull) return branches;

        // Parse the search results
        var resultsArray = (RedisResult[])searchResults;
        for (int i = 1; i < resultsArray.Length; i++) // Skipping the total count, directly accessing keys
        {
            var key = (string)resultsArray[i];
            var hashEntries = await _cacheDb.HashGetAllAsync(key);
            if (hashEntries.Length > 0)
            {
                // Temporary variables to hold hash values
                Guid id = Guid.Empty;
                string name = null, repositoryId = null, issueId = null;
                List<Commit> commits = new List<Commit>(); // Assuming you have a Commit class to deserialize into

                // Extract values from hash entries
                foreach (var entry in hashEntries)
                {
                    switch (entry.Name.ToString())
                    {
                        case "Id":
                            id = Guid.Parse(entry.Value);
                            break;
                        case "Name":
                            name = entry.Value;
                            break;
                        case "RepositoryId":
                            repositoryId = entry.Value;
                            break;
                        case "IssueId":
                            issueId = entry.Value; // Assuming IssueId is nullable or optional
                            break;
                        case "Commits":
                            commits = JsonConvert.DeserializeObject<List<Commit>>(entry.Value); // Deserialize the JSON string into a List<Commit>
                            break;
                    }
                }

                if (id != Guid.Empty && name != null)
                {
                    var branch = Core.Models.Entities.Branch.Create(
                        name, 
                        Guid.Parse(repositoryId is null ? "0000-0000-0000-0000" : repositoryId), 
                        Guid.Parse(issueId is null ? "0000-0000-0000-0000" : issueId),
                        commits, 
                        id);
                    branches.Add(branch);
                }
            }
        }

        return branches;
    }

    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        var comments = new List<Comment>();
        var indexName = "idx:Comment";

        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return comments;

        var resultsArray = (RedisResult[])searchResults;
        var totalResults = (long)resultsArray[0];

        for (int i = 1; i <= totalResults; i++)
        {
            var key = (string)resultsArray[i];
            var commentData = await _cacheDb.StringGetAsync(key);
            if (!commentData.IsNull)
            {
                var comment = System.Text.Json.JsonSerializer.Deserialize<Comment>(commentData);
                comments.Add(comment);
            }
        }

        return comments;
    }
    public async Task<List<Label>> GetAllLabelsAsync()
    {
        var labels = new List<Label>();
        var indexName = "idx:Label";

        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return labels;

        var resultsArray = (RedisResult[])searchResults;
        var totalResults = (long)resultsArray[0];

        for (int i = 1; i <= totalResults; i++)
        {
            var key = (string)resultsArray[i];
            var labelData = await _cacheDb.StringGetAsync(key);
            if (!labelData.IsNull)
            {
                var label = System.Text.Json.JsonSerializer.Deserialize<Label>(labelData);
                labels.Add(label);
            }
        }

        return labels;
    }
    public async Task<List<Milestone>> GetAllMilestonesAsync()
    {
        var milestones = new List<Milestone>();
        var indexName = "idx:Milestone";

        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return milestones;

        var resultsArray = (RedisResult[])searchResults;
        var totalResults = (long)resultsArray[0];

        for (int i = 1; i <= totalResults; i++)
        {
            var key = (string)resultsArray[i];
            var milestoneData = await _cacheDb.StringGetAsync(key);
            if (!milestoneData.IsNull)
            {
                var milestone = System.Text.Json.JsonSerializer.Deserialize<Milestone>(milestoneData);
                milestones.Add(milestone);
            }
        }

        return milestones;
    }
    public async Task<List<Core.Models.AggregateRoots.User>> GetAllUsersAsync()
    {
        var users = new List<Core.Models.AggregateRoots.User>();
        var indexName = "idx:User";

        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return users;

        var resultsArray = (RedisResult[])searchResults;
        var totalResults = (long)resultsArray[0];

        for (int i = 1; i <= totalResults; i++)
        {
            var key = (string)resultsArray[i];
            var userData = await _cacheDb.StringGetAsync(key);
            if (!userData.IsNull)
            {
                var user = System.Text.Json.JsonSerializer.Deserialize<Core.Models.AggregateRoots.User>(userData);
                users.Add(user);
            }
        }

        return users;
    }
    //public T GetData<T>(string key)
    //{
    //    var value = _cacheDb.StringGet(key);
    //    if (!string.IsNullOrEmpty(value))
    //    {
    //        return JsonSerializer.Deserialize<T>(value);
    //    }
    //    return default;
    //}

    //public object RemoveData(string key)
    //{
    //    var exist = _cacheDb.KeyExists(key);

    //    if (exist)
    //    {
    //        return _cacheDb.KeyDelete(key);
    //    }
    //    return false;
    //}
    #endregion

    #region removedata
    public async Task RemoveAllRepositoryDataAsync()
    {
        var indexName = "idx:Repository";
        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return;

        var resultsArray = (RedisResult[])searchResults;
        // Skip the first element as it contains the total count
        for (int i = 1; i < resultsArray.Length; i++)
        {
            var key = (string)resultsArray[i];
            // Remove the actual data associated with each key without touching the index
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
    public async Task RemoveAllIssueDataAsync()
    {
        var indexName = "idx:Issue";
        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return;

        var resultsArray = (RedisResult[])searchResults;
        // Skip the first element as it contains the total count
        for (int i = 1; i < resultsArray.Length; i++)
        {
            var key = (string)resultsArray[i];
            // Remove the actual data associated with each key without touching the index
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
    public async Task RemoveAllBranchDataAsync()
    {
        var indexName = "idx:Branch";
        var searchResults = await _cacheDb.ExecuteAsync("FT.SEARCH", indexName, "*", "NOCONTENT");
        if (searchResults.IsNull) return;

        var resultsArray = (RedisResult[])searchResults;
        // Skip the first element as it contains the total count
        for (int i = 1; i < resultsArray.Length; i++)
        {
            var key = (string)resultsArray[i];
            // Remove the actual data associated with each key without touching the index
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
    #endregion
}
