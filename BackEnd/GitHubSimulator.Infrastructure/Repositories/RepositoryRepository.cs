using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.Enums;
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

    public async Task<IEnumerable<Repository>> GetPublicRepositories(int page, int limit)
    {
        return await _repositoryCollection
            .Find(r => r.Visibility == Visibility.Public)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();
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
            .Set(x => x.Name, updatedRepository.Name)
            .Set(x => x.Visibility, updatedRepository.Visibility);

        var result = await _repositoryCollection.UpdateOneAsync(filter, updateDefinition);

        return result.MatchedCount > 0 ? Maybe.From(updatedRepository) : Maybe.None;
    }

    public async Task<bool> Delete(string repositoryName)
    {
        var filter = Builders<Repository>.Filter.Eq(x => x.Name, repositoryName);
        var result = await _repositoryCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    public async Task<Maybe<Repository>> UpdateName(string repositoryName, string newName)
    {
        var filter = Builders<Repository>.Filter.Eq(x => x.Name, repositoryName);
        var updateDefinition = Builders<Repository>.Update
            .Set(x => x.Name, newName);
        
        var options = new FindOneAndUpdateOptions<Repository>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };
        
        var result = await _repositoryCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
    
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<Maybe<Repository>> UpdateVisibility(string repositoryName, bool isPrivate)
    {
        var newVisibility = isPrivate ? Visibility.Private : Visibility.Public;
        
        var filter = Builders<Repository>.Filter.Eq(x => x.Name, repositoryName);
        var updateDefinition = Builders<Repository>.Update
            .Set(x => x.Visibility, newVisibility);
        
        var options = new FindOneAndUpdateOptions<Repository>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };
        
        var result = await _repositoryCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
    
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<Maybe<Repository>> UpdateRepositoryOwner(string repositoryName, string newOwner)
    {
        var filter = Builders<Repository>.Filter.Eq(x => x.Name, repositoryName);
        var updateDefinition = Builders<Repository>.Update
            .Set(x => x.Owner, newOwner);

        var options = new FindOneAndUpdateOptions<Repository>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };

        var result = await _repositoryCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);
        return result != null ? Maybe.From(result) : Maybe.None;
    }

    public async Task<string> GetRepositoryOwner(string repo)
    {
        var repository = await _repositoryCollection.Find(x => x.Name.Equals(repo)).FirstOrDefaultAsync();
        return repository.Owner;
    }

    public async Task<Repository> GetByName(string name)
    {
        var repository = await _repositoryCollection.Find(x => x.Name.Equals(name)).FirstOrDefaultAsync();

        return repository;
    }
}