using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories;

public class UserRepositoryRepository : IUserRepositoryRepository
{
    private readonly IMongoCollection<Core.Models.Entities.UserRepository> _userRepositoryCollection;
    
    public UserRepositoryRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _userRepositoryCollection =
            mongoDatabase.GetCollection<Core.Models.Entities.UserRepository>(dbSettings.Value.UserRepositoryCollectionName);
    }
    
    public async Task<IEnumerable<Core.Models.Entities.UserRepository>> GetAll()
    {
        return await _userRepositoryCollection.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Core.Models.Entities.UserRepository>> GetByUserName(string userName)
    {
        var filter = Builders<Core.Models.Entities.UserRepository>.Filter
            .Eq(x => x.UserName, userName);

        var result = await _userRepositoryCollection.Find(filter).ToListAsync();

        return result ?? new List<Core.Models.Entities.UserRepository>();
    }

    public async Task<IEnumerable<Core.Models.Entities.UserRepository>> GetByRepositoryName(string repoName)
    {
        var filter = Builders<Core.Models.Entities.UserRepository>.Filter
            .Eq(x => x.RepositoryName, repoName);

        var result = await _userRepositoryCollection.Find(filter).ToListAsync();

        return result ?? new List<Core.Models.Entities.UserRepository>();
    }

    public async Task<Core.Models.Entities.UserRepository> AddUserToRepository(Core.Models.Entities.UserRepository userRepository)
    {
        await _userRepositoryCollection.InsertOneAsync(userRepository);
        return userRepository;
    }

    public async Task<bool> RemoveUserFromRepository(string userName, string repoName)
    {
        var filter = Builders<Core.Models.Entities.UserRepository>.Filter
            .Eq(x => x.UserName, userName) & Builders<Core.Models.Entities.UserRepository>.Filter
            .Eq(x => x.RepositoryName, repoName);

        var result = await _userRepositoryCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    public async Task<Maybe<Core.Models.Entities.UserRepository>> ChangeUserRole(string userName, string repoName, UserRepositoryRole newRole)
    {
        var filter = Builders<Core.Models.Entities.UserRepository>.Filter
            .Eq(x => x.UserName, userName) & Builders<Core.Models.Entities.UserRepository>.Filter
            .Eq(x => x.RepositoryName, repoName);

        var updateDefinition = Builders<Core.Models.Entities.UserRepository>.Update
            .Set(x => x.UserRepositoryRole, newRole);

        var options = new FindOneAndUpdateOptions<Core.Models.Entities.UserRepository>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false,
        };

        var result = await _userRepositoryCollection.FindOneAndUpdateAsync(filter, updateDefinition, options);

        return result != null ? Maybe.From(result) : Maybe.None;
    }
}