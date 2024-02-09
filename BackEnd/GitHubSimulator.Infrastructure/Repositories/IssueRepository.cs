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
        

    public IssueRepository(IOptions<DatabaseSettings> dbSettings, IMilestoneRepository milestoneRepository)
    {
        _milestoneRepository = milestoneRepository;
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
            .Set(x => x.Assigne, updatedIssue.Assigne)
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
