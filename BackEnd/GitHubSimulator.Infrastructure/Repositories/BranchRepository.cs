using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly IMongoCollection<Branch> _branchCollection;

    public BranchRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _branchCollection = 
            mongoDatabase.GetCollection<Branch>(dbSettings.Value.BranchCollectionName);
    }

    public async Task<Branch> GetById(Guid id)
    {
        return await _branchCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Branch>> GetAll()
    {
        return await _branchCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Branch> Insert(Branch branch)
    {
        await _branchCollection.InsertOneAsync(branch);
        return branch;
    }

    public async Task<Maybe<Branch>> Update(Branch updatedBranch)
    {
        var filter = Builders<Branch>.Filter.Eq(x => x.Id, updatedBranch.Id);
        var updatedDefinition = Builders<Branch>.Update
            .Set(x => x.Name, updatedBranch.Name);
            

        if (updatedBranch.IssueId is not null)
        {
            updatedDefinition = updatedDefinition.Set(x => x.IssueId, updatedBranch.IssueId);
        }
        
        var result = await _branchCollection.UpdateOneAsync(filter, updatedDefinition);

        return result.MatchedCount > 0 ? Maybe.From(await GetById(updatedBranch.Id)) : Maybe.None;
    }

    public async Task<bool> Delete(Guid branchId)
    {
        var filter = Builders<Branch>.Filter.Eq(x => x.Id, branchId);
        var result = await _branchCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}