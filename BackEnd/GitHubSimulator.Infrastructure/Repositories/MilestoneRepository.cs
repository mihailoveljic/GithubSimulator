﻿using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
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
        var milestoneIdFilter = Builders<Milestone>.Filter.Eq(milestone => milestone.Id, milestoneId);
        var milestone = await _milestoneCollection.Find(milestoneIdFilter).FirstOrDefaultAsync();

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
}
