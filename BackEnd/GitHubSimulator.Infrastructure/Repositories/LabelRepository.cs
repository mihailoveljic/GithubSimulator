using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories
{
    public class LabelRepository : ILabelRepository
    {
        private readonly IMongoCollection<Label> _labelCollection;

        public LabelRepository(IOptions<DatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _labelCollection = mongoDatabase.GetCollection<Label>(dbSettings.Value.LabelCollectionName);
        }

        public async Task<IEnumerable<Label>> GetAll()
        {
            return await _labelCollection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Label>> SearchLabels(string searchString)
        {
            if (searchString.Equals("")) return await GetAll();
            
            var filter = Builders<Label>.Filter.Where(label =>
                label.Name.ToLower().Contains(searchString.ToLower()) 
                || label.Description.ToLower().Contains(searchString.ToLower()));

            var labels = await _labelCollection.Find(filter).ToListAsync();
            return labels;
        }

        public async Task<Maybe<Label>> GetById(Guid id)
        {
            var filter = Builders<Label>.Filter.Eq(x => x.Id, id);
            var result = await _labelCollection.Find(filter).FirstOrDefaultAsync();
            return result is not null ? Maybe.From(result) : null;
        }

        public async Task<Label> Insert(Label label)
        {
            await _labelCollection.InsertOneAsync(label);
            return label;
        }

        public async Task<Maybe<Label>> Update(Label updatedLabel)
        {
            var filter = Builders<Label>.Filter.Eq(x => x.Id, updatedLabel.Id);
            var updateDefinition = Builders<Label>.Update
                .Set(x => x.Name, updatedLabel.Name)
                .Set(x => x.Description, updatedLabel.Description)
                .Set(x => x.Color, updatedLabel.Color);

            var result = await _labelCollection.UpdateOneAsync(filter, updateDefinition);

            return result.MatchedCount > 0 ? Maybe.From(updatedLabel) : Maybe.None;
        }

        public async Task<bool> Delete(Guid labelId)
        {
            var filter = Builders<Label>.Filter.Eq(x => x.Id, labelId);
            var result = await _labelCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
    }
}
