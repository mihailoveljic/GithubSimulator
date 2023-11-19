//using GitHubSimulator.Core.Interfaces;
//using GitHubSimulator.Core.Models;
//using GitHubSimulator.Infrastructure.Configuration;
//using Microsoft.Extensions.Options;
//using MongoDB.Driver;

//namespace GitHubSimulator.Infrastructure.Repositories;

//public class IssueRepository : IIssueRepository
//{
//    private readonly IMongoCollection<Issue> _issueCollection;

//    public IssueRepository(IOptions<DatabaseSettings> dbSettings)
//    {
//        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

//        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

//        _issueCollection = mongoDatabase.GetCollection<Issue>(dbSettings.Value.IssueCollectionName);
//    }

//    public async Task<IEnumerable<Issue>> GetAll()
//    {
//        return await _issueCollection.Find(_ => true).ToListAsync();
//    }
//}
