using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories;

public class IssueRepository : IIssueRepository
{
    private readonly IMongoCollection<Issue> _issueCollection;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly ILabelRepository _labelRepository;
        

    public IssueRepository(IOptions<DatabaseSettings> dbSettings, IMilestoneRepository milestoneRepository, ILabelRepository labelRepository)
    {
        _milestoneRepository = milestoneRepository;
        _labelRepository = labelRepository;
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _issueCollection = mongoDatabase.GetCollection<Issue>(dbSettings.Value.IssueCollectionName);
    }

    public async Task<IEnumerable<Issue>> GetAll()
    {
        return await _issueCollection.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Issue>> GetIssuesForMilestone(Guid milestoneId)
    {
        return await _issueCollection.Find(x => x.MilestoneId.Equals(milestoneId)).ToListAsync();
    }

    public async Task<IEnumerable<Issue>> SearchIssues(string searchString, string email)
    {
        if (searchString.Equals("") || searchString.Equals("q:")) return await GetAll();
        if (searchString.Contains("q:")) return new List<Issue>();
        
        var keyValuePairs = searchString.Split(' ');
        var filterBuilder = Builders<Issue>.Filter;
        var filter = Builders<Issue>.Filter.Empty;
        SortDefinition<Issue>? sortDefinition = null;
        var isSortingByUpdatedDate = 0;
        
        foreach (var pair in keyValuePairs)
        {
            var parts = pair.Split(':');
            if (parts.Length == 2)
            {
                var key = parts[0];
                var value = parts[1];

                switch (key.ToLower())
                {
                    case "assignee":
                        filter &= value switch
                        {
                            "" => filterBuilder.Eq(issue => issue.Assigne.Email, null),
                            "@me" => filterBuilder.Eq(issue => issue.Assigne.Email, email),
                            _ => filterBuilder.Eq(issue => issue.Assigne.Email, value)
                        };
                        break;
                    case "author":
                        filter &= value switch
                        {
                            "" => filterBuilder.Eq(issue => issue.Author.Email, null),
                            "@me" => filterBuilder.Eq(issue => issue.Author.Email, email),
                            _ => filterBuilder.Eq(issue => issue.Author.Email, value)
                        };
                        break;
                    case "milestone":
                        if (value.Equals(""))
                        {
                            filter &= filterBuilder.Eq(issue => issue.MilestoneId, null);
                        }
                        else
                        {
                            value = value.Replace("_", " ");
                            var(hasValue, milestoneResult) = 
                                await _milestoneRepository.GetByTitle(value);
                            if (!hasValue)
                            {
                                return new List<Issue>();
                            }

                            filter &= filterBuilder.Eq(issue => issue.MilestoneId, milestoneResult.Id);
                        }
                        break;
                    case "label":
                        if (value.Equals(""))
                        {
                            filter &= filterBuilder.Or(
                                filterBuilder.Size(issue => issue.Labels, 0),
                                filterBuilder.Eq(issue => issue.Labels, null)
                            );
                        }
                        else
                        {
                            value = value.Replace("_", " ");
                            filter &= filterBuilder.AnyEq(issue => issue.Labels.Select(label => label.Name), value);
                        }
                        break;
                    case "is":
                        switch (value)
                        {
                            case "open":
                                filter &= filterBuilder.Eq(issue => issue.IsOpen, true);
                                break;
                            case "closed":
                                filter &= filterBuilder.Eq(issue => issue.IsOpen, false);
                                break;
                            default:
                                return new List<Issue>();
                        }
                        break;
                    case "sort":
                        switch (value)
                        {
                            case "created-desc":
                                sortDefinition = Builders<Issue>.Sort.Descending(
                                    issue => issue.CreatedAt);
                                break;
                            case "created-asc":
                                sortDefinition = Builders<Issue>.Sort.Ascending(
                                    issue => issue.CreatedAt);
                                break;
                            case "comments-desc":
                                break;
                            case "comments-asc":
                                break;
                            case "updated-desc":
                                isSortingByUpdatedDate = 1;
                                break;
                            case "updated-asc":
                                isSortingByUpdatedDate = 2;
                                break;
                        }
                        break;
                    default:
                        return new List<Issue>();
                }
            }
        }

        switch (isSortingByUpdatedDate)
        {
            case 1:
                var issues = await _issueCollection.Find(filter).ToListAsync();
                issues = issues.OrderByDescending(
                    issue => (issue.Events ?? Array.Empty<Event>()).Max(ev => ev.DateTimeOccured)).ToList();
                return issues;
            case 2:
                var issues1 = await _issueCollection.Find(filter).ToListAsync();
                issues1 = issues1.OrderBy(
                    issue => (issue.Events ?? Array.Empty<Event>()).Max(ev => ev.DateTimeOccured)).ToList();
                return issues1;
            default:
                var query = _issueCollection.Find(filter);
                if (sortDefinition != null)
                {
                    query = query.Sort(sortDefinition);
                }
                return await query.ToListAsync();
        }
    }

    public async Task<Maybe<Issue>> GetById(Guid id)
    {
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, id);
        var result = await _issueCollection.Find(filter).FirstOrDefaultAsync();
        return result is not null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<Issue> Insert(Issue issue)
    {
        await _issueCollection.InsertOneAsync(issue);
        return issue;
    }

    public async Task<Maybe<Issue>> Update(Issue updatedIssue)
    {
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, updatedIssue.Id);
        var updateDefinition = Builders<Issue>.Update
            .Set(x => x.Title, updatedIssue.Title)
            .Set(x => x.Description, updatedIssue.Description)
            .Set(x => x.CreatedAt, updatedIssue.CreatedAt)
            .Set(x => x.Assignee, updatedIssue.Assignee)
            .Set(x => x.RepositoryId, updatedIssue.RepositoryId)
            .Set(x => x.MilestoneId, updatedIssue.MilestoneId);

        var result = await _issueCollection.UpdateOneAsync(filter, updateDefinition);

        return result.ModifiedCount > 0 ? Maybe.From(updatedIssue) : Maybe.None;
    }

    public async Task<Maybe<Issue>> UpdateIssueTitle(Guid id, string title, string email)
    {
        var (hasValue, issueResult) = await GetById(id);
        if (!hasValue)
        {
            return Maybe.None;
        }

        var issueEvents = issueResult.Events?.ToList() ?? new List<Event>();
        issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
            email + " changed the Issue title at " + DateTime.Now));
        
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, id);
        var updateDefinition = Builders<Issue>.Update
            .Set(x => x.Title, title)
            .Set(x => x.Events, issueEvents);
        
        var options = new FindOneAndUpdateOptions<Issue>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };
        
        var result = await _issueCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<Maybe<Issue>> UpdateIssueMilestone(Guid id, Guid? milestoneId, string email)
    {
        var (hasValue, issueResult) = await GetById(id);
        if (!hasValue)
        {
            return Maybe.None;
        }
        
        var issueEvents = issueResult.Events?.ToList() ?? new List<Event>();
        
        if (milestoneId == null)
        {
            if (issueResult.MilestoneId == null)
            {
                return Maybe.None;
            }

            var oldMilestoneMaybe = await _milestoneRepository.GetById(issueResult.MilestoneId.Value);
            var oldMilestone = oldMilestoneMaybe.Value;
            
            issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                email + " removed this issue from the " + oldMilestone.Title + " milestone at " + DateTime.Now));
        }
        else
        {
            var (hasValueMilestone, milestoneResult) = await _milestoneRepository.GetById(milestoneId.Value);
            if (!hasValueMilestone)
            {
                return Maybe.None;
            }
        
            issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                email + " added this to the " + milestoneResult.Title + " milestone at " + DateTime.Now));
        }
        
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, id);
        var updateDefinition = Builders<Issue>.Update
            .Set(x => x.MilestoneId, milestoneId)
            .Set(x => x.Events, issueEvents);
        
        var options = new FindOneAndUpdateOptions<Issue>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };
        
        var result = await _issueCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<Maybe<Issue>> UpdateIssueAssignee(Guid id, string? assignee, string email)
    {
        var (hasValue, issueResult) = await GetById(id);
        if (!hasValue)
        {
            return Maybe.None;
        }
        
        var issueEvents = issueResult.Events?.ToList() ?? new List<Event>();
        
        if (assignee == null)
        {
            if (issueResult.Assigne.Email == null)
            {
                return Maybe.None;
            }

            if (email.Equals(issueResult.Assigne.Email))
            {
                issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                    email + " un-assigned themselves from this issue at " + DateTime.Now));
            }
            else
            {
                issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                    email + " un-assigned " + issueResult.Assigne.Email + " from this issue at " + DateTime.Now));
            }
        }
        else
        {
            if (email.Equals(assignee))
            {
                issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                    email + " self-assigned this issue at " + DateTime.Now));
            }
            else
            {
                issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                    email + " assigned " + assignee +  " to this issue at " + DateTime.Now));
            }
        }
        
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, id);
        var updateDefinition = Builders<Issue>.Update
            .Set(x => x.Assigne.Email, assignee)
            .Set(x => x.Events, issueEvents);

        var options = new FindOneAndUpdateOptions<Issue>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };

        var result = await _issueCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<Maybe<Issue>> UpdateIssueLabels(Guid issueId, string userEmail, List<Guid> labelIds)
    {
        var (hasValue, issueResult) = await GetById(issueId);
        if (!hasValue)
        {
            return Maybe.None;
        }
        
        var issueLabels = issueResult.Labels?.ToList() ?? new List<Label>();
        var issueEvents = issueResult.Events?.ToList() ?? new List<Event>();
        
        var newlyAssignedLabels = new List<Label>();
        
        foreach (var labelId in labelIds)
        {
            var labelMaybe = await _labelRepository.GetById(labelId);
            var label = labelMaybe.Value;
            newlyAssignedLabels.Add(label);
        }

        var removedLabels = issueLabels
            .Select(label => label.Name)
            .Except(newlyAssignedLabels.Select(label => label.Name))
            .ToList();

        var newLabels = newlyAssignedLabels
            .Select(label => label.Name)
            .Except(issueLabels.Select(label => label.Name))
            .ToList();

        var eventDescription = !removedLabels.Any() ? "" : userEmail + " removed ";
        eventDescription = removedLabels.Aggregate(eventDescription, (current, lab) => current + (lab + " "));

        if (!eventDescription.Equals(""))
        { 
            eventDescription += " from the issue ";
        }

        if (newLabels.Any()) eventDescription += userEmail + " assigned ";
        
        eventDescription = newLabels.Aggregate(eventDescription, (current, lab1) => current + " " + lab1);
        if (newLabels.Any()) eventDescription += " to the issue";

        if (newLabels.Any() || !eventDescription.Equals("")) issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
            eventDescription + " at " + DateTime.Now));
        
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, issueId);
        var updateDefinition = Builders<Issue>.Update
            .Set(x => x.Labels, newlyAssignedLabels)
            .Set(x => x.Events, issueEvents);
        
        var options = new FindOneAndUpdateOptions<Issue>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };
        
        var result = await _issueCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<Maybe<Issue>> OpenOrCloseIssue(Guid id, bool isOpen, string email)
    {
        var (hasValue, issueResult) = await GetById(id);
        if (!hasValue)
        {
            return Maybe.None;
        }
        
        var issueEvents = issueResult.Events?.ToList() ?? new List<Event>();

        if (isOpen)
        {
            issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                email + " re-opened this issue at " + DateTime.Now));
        }
        else
        {
            issueEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                email + " closed this issue at " + DateTime.Now));
        }
        
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, id);
        var updateDefinition = Builders<Issue>.Update
            .Set(x => x.IsOpen, isOpen)
            .Set(x => x.Events, issueEvents);
        
        var options = new FindOneAndUpdateOptions<Issue>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };
        
        var result = await _issueCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<bool> Delete(Guid issueId)
    {
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, issueId);
        var result = await _issueCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}
