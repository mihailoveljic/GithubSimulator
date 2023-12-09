using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories;

public class IssueRepository : IIssueRepository
{
    private readonly IMongoCollection<Issue> _issueCollection;

    public IssueRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _issueCollection = mongoDatabase.GetCollection<Issue>(dbSettings.Value.IssueCollectionName);
    }

    public async Task<Issue> GetById(Guid id)
    {
        return await _issueCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Issue>> GetAll()
    {
        return await _issueCollection.Find(_ => true).ToListAsync();
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

    public async Task<bool> Delete(Guid issueId)
    {
        var filter = Builders<Issue>.Filter.Eq(x => x.Id, issueId);
        var result = await _issueCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}
