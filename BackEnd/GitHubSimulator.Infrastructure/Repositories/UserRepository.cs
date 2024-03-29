﻿using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GitHubSimulator.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _userCollection;
    private readonly IUserRepositoryRepository _userRepositoryRepository;

    public UserRepository(IOptions<DatabaseSettings> dbSettings, IUserRepositoryRepository userRepositoryRepository)
    {
        _userRepositoryRepository = userRepositoryRepository;
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _userCollection = mongoDatabase.GetCollection<User>(dbSettings.Value.UserCollectionName);
    }

    public async Task<User> Insert(User user)
    {
        await _userCollection.InsertOneAsync(user);
        return user;
    }

    public async Task<User> GetById(Guid userId)
    {
        return await _userCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userCollection.Find(_ => true).ToListAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        return await _userCollection.Find(x => x.Mail.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> GetByUsername(string username)
    {
        return await _userCollection.Find(x => x.AccountCredentials.UserName == username).FirstOrDefaultAsync();
    }

    public async Task<Maybe<User>> Update(User updatedUser)
    {
        var oldUser = await _userCollection.Find(x => x.Id == updatedUser.Id).FirstOrDefaultAsync();

        if (oldUser is null)
        {
            return Maybe.None;
        }

        var filter = Builders<User>.Filter.Eq(x => x.Id, updatedUser.Id);
        var updateDefinition = Builders<User>.Update
            .Set(x => x.Name, updatedUser.Name)
            .Set(x => x.Surname, updatedUser.Surname)
            .Set(x => x.Mail, updatedUser.Mail)
            .Set(x => x.AccountCredentials, AccountCredentials.Create(updatedUser.AccountCredentials.UserName, oldUser.AccountCredentials.PasswordHash));

        var result = await _userCollection.UpdateOneAsync(filter, updateDefinition);

        return result.ModifiedCount > 0 ? Maybe.From(updatedUser) : Maybe.None;
    }

    public async Task<bool> UpdatePassword(Guid userId, string newPassword)
    {
        var user = await _userCollection.Find(x => x.Id == userId).FirstOrDefaultAsync();

        var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
        var updateDefinition = Builders<User>.Update
            .Set(x => x.AccountCredentials, AccountCredentials.Create(user.AccountCredentials.UserName, newPassword));

        var result = await _userCollection.UpdateOneAsync(filter, updateDefinition);

        return result.ModifiedCount > 0;
    }

    public async Task<IEnumerable<User>> GetUsersNotInRepository(string repoName, string searchString)
    {
        var userRepositoriesEnumerable = await _userRepositoryRepository.GetByRepositoryName(repoName);
        var userRepositories = userRepositoriesEnumerable.ToList();
        
        if (!userRepositories.Any()) return await GetAll();

        var usernames = userRepositories.Select(ur => ur.UserName).ToList();

        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Not(Builders<User>.Filter.In(u => u.AccountCredentials.UserName, usernames)),
            Builders<User>.Filter.Or(
Builders<User>.Filter.Regex(u => u.AccountCredentials.UserName, new BsonRegularExpression(searchString, "i")),
            Builders<User>.Filter.Regex(u => u.Mail.Email, new BsonRegularExpression(searchString, "i")),
            Builders<User>.Filter.Regex(u => u.Name, new BsonRegularExpression(searchString, "i")),
            Builders<User>.Filter.Regex(u => u.Surname, new BsonRegularExpression(searchString, "i"))));
        
        var filteredUsers = await _userCollection.Find(filter).ToListAsync();
        return filteredUsers;
    }

    public async Task<bool> Delete(Guid userId)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, userId);
        var result = await _userCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}
