using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Dtos.Milestones;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Specifications;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories;

public class MilestoneRepository : IMilestoneRepository
{
    private readonly IMongoCollection<Issue> _issueCollection;
    private readonly IMongoCollection<Milestone> _milestoneCollection;

    public MilestoneRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _issueCollection = mongoDatabase.GetCollection<Issue>(dbSettings.Value.IssueCollectionName);
        _milestoneCollection = mongoDatabase.GetCollection<Milestone>(dbSettings.Value.MilestoneCollectionName);
    }

    public async Task<IEnumerable<Milestone>> GetAll()
    {
        return await _milestoneCollection.Find(_ => true).ToListAsync();
    }

    public async Task<MilestoneWithIssues> GetMilestoneWithIssues(Specification<Issue> specification, Guid milestoneId)
    {
        var milestone = await _milestoneCollection.Find(x => x.Id == milestoneId).FirstOrDefaultAsync();

        var issues = await _issueCollection.Find(_ => true).ToListAsync();
        return new MilestoneWithIssues(
            milestone.Id,
            milestone.Title,
            milestone.Description,
            milestone.DueDate,
            milestone.State,
            milestone.RepositoryId,
            issues.Where(specification.IsSatisfiedBy));
    }

    public async Task<Milestone> Insert(Milestone milestone)
    {
        await _milestoneCollection.InsertOneAsync(milestone);
        return milestone;
    }

    public async Task<Maybe<Milestone>> Update(Milestone updatedMilestone)
    {
        var filter = Builders<Milestone>.Filter.Eq(x => x.Id, updatedMilestone.Id);
        var updateDefinition = Builders<Milestone>.Update
            .Set(x => x.Title, updatedMilestone.Title)
            .Set(x => x.Description, updatedMilestone.Description)
            .Set(x => x.DueDate, updatedMilestone.DueDate)
            .Set(x => x.State, updatedMilestone.State)
            .Set(x => x.RepositoryId, updatedMilestone.RepositoryId);

        var result = await _milestoneCollection.UpdateOneAsync(filter, updateDefinition);

        return result.ModifiedCount > 0 ? Maybe.From(updatedMilestone) : Maybe.None;
    }

    public async Task<bool> Delete(Guid milestoneId)
    {
        var filter = Builders<Milestone>.Filter.Eq(x => x.Id, milestoneId);
        var result = await _milestoneCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

}
