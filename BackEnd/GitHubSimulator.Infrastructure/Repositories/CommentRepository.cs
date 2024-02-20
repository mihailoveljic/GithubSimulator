using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly IMongoCollection<Comment> _commentCollection;

    public CommentRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

        _commentCollection = mongoDatabase.GetCollection<Comment>(dbSettings.Value.CommentCollectionName);
    }

    public async Task<Comment> GetById(Guid id)
    {
        return await _commentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Comment>> GetAll()
    {
        return await _commentCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Comment> Insert(Comment comment)
    {
        await _commentCollection.InsertOneAsync(comment);
        return comment;
    }

    public async Task<Maybe<Comment>> Update(Comment updatedComment)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.Id, updatedComment.Id);
        var updateDefinition = Builders<Comment>.Update
            .Set(x => x.Content, updatedComment.Content);

        var result = await _commentCollection.UpdateOneAsync(filter, updateDefinition);

        return result.MatchedCount > 0 ? Maybe.From(await GetById(updatedComment.Id)) : Maybe.None;
    }

    public async Task<bool> Delete(Guid id)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.Id, id);
        var result = await _commentCollection.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
}