using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories;

public class RepositoryRepository : IRepositoryRepository
{
    private readonly IMongoCollection<Repository> _repositoryCollection;

    public RepositoryRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _repositoryCollection = mongoDatabase.GetCollection<Repository>(dbSettings.Value.RepositoryCollectionName);
    }

    public async Task<Repository> GetById(Guid id)
    {
        return await _repositoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Repository>> GetAll()
    {
        return await _repositoryCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Repository> Insert(Repository repository)
    {
        await  _repositoryCollection.InsertOneAsync(repository);
        return repository;
    }

    public async Task<Maybe<Repository>> Update(Repository updatedRepository)
    {
        var filter = Builders<Repository>.Filter.Eq(x => x.Id, updatedRepository.Id);
        var updateDefinition = Builders<Repository>.Update
            .Set(x => x.Description, updatedRepository.Description)
            .Set(x => x.Name, updatedRepository.Name);

        var result = await _repositoryCollection.UpdateOneAsync(filter, updateDefinition);

        return result.MatchedCount > 0 ? Maybe.From(updatedRepository) : Maybe.None;
    }

    public async Task<bool> Delete(Guid repositoryId)
    {
        var filter = Builders<Repository>.Filter.Eq(x => x.Id, repositoryId);
        var result = await _repositoryCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}