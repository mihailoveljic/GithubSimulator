using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories
{
    public class PullRequestRepository : IPullRequestRepository
    {
        private readonly IMongoCollection<PullRequest> _pullRequestCollection;

        public PullRequestRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _pullRequestCollection = mongoDatabase.GetCollection<PullRequest>(dbSettings.Value.PullRequestCollectionName);
        }

        public async Task<IEnumerable<PullRequest>> GetAll()
        {
            return await _pullRequestCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Maybe<PullRequest>> GetById(Guid pullRequestId)
        {
            var filter = Builders<PullRequest>.Filter.Eq(x => x.Id, pullRequestId);
            var result = await _pullRequestCollection.Find(filter).FirstOrDefaultAsync();
            return result is not null ? Maybe.From(result) : Maybe.None; 
        }

        public async Task<PullRequest> Insert(PullRequest pullRequest)
        {
            await _pullRequestCollection.InsertOneAsync(pullRequest);
            return pullRequest;
        }

        public async Task<Maybe<PullRequest>> Update(PullRequest updatedPullRequest)
        {
            var filter = Builders<PullRequest>.Filter.Eq(x => x.Id, updatedPullRequest.Id);
            var updateDefinition = Builders<PullRequest>.Update
                .Set(x => x.Events, updatedPullRequest.Events)
                .Set(x => x.Target, updatedPullRequest.Target)
                .Set(x => x.Source, updatedPullRequest.Source)
                .Set(x => x.IssueId, updatedPullRequest.IssueId)
                .Set(x => x.TaskType, updatedPullRequest.TaskType)
                .Set(x => x.MilestoneId, updatedPullRequest.MilestoneId);

            var result = await _pullRequestCollection.UpdateOneAsync(filter, updateDefinition);

            return result.ModifiedCount > 0 ? Maybe.From(updatedPullRequest) : Maybe.None;
        }

        public async Task<bool> Delete(Guid pullRequestId)
        {
            var filter = Builders<PullRequest>.Filter.Eq(x => x.Id, pullRequestId);
            var result = await _pullRequestCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
    }
}
