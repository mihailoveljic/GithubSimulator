using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Repositories;
using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GitHubSimulator.Infrastructure.Repositories
{
    public class PullRequestRepository : IPullRequestRepository
    {
        private readonly IMongoCollection<PullRequest> _pullRequestCollection;
        private readonly IMilestoneRepository _milestoneRepository;
        private readonly ILabelRepository _labelRepository;

        public PullRequestRepository(IOptions<DatabaseSettings> dbSettings, IMilestoneRepository milestoneRepository, ILabelRepository labelRepository)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _pullRequestCollection = mongoDatabase.GetCollection<PullRequest>(dbSettings.Value.PullRequestCollectionName);
            _milestoneRepository = milestoneRepository;
            _labelRepository = labelRepository;
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

        public async Task<Maybe<PullRequest>> Update(PullRequest updatedPullRequest, string user)
        {
            var pullEvents = updatedPullRequest.Events?.ToList() ?? new List<Event>();
            pullEvents.Add(new Event(Guid.NewGuid(), DateTime.Now, EventType.StateChange,
                user + " changed the Pull Request " + DateTime.Now));

            var filter = Builders<PullRequest>.Filter.Eq(x => x.Id, updatedPullRequest.Id);
            var updateDefinition = Builders<PullRequest>.Update
                .Set(x => x.Events, pullEvents)
                .Set(x => x.Target, updatedPullRequest.Target)
                .Set(x => x.Title, updatedPullRequest.Title)
                .Set(x => x.Source, updatedPullRequest.Source)
                .Set(x => x.IssueId, updatedPullRequest.IssueId)
                .Set(x => x.TaskType, updatedPullRequest.TaskType)
                .Set(x => x.MilestoneId, updatedPullRequest.MilestoneId)
                .Set(x => x.IsOpen, updatedPullRequest.IsOpen)
                .Set(x => x.Labels, updatedPullRequest.Labels)
                .Set(x => x.Assignee, updatedPullRequest.Assignee)
                .Set(x => x.Assignees, updatedPullRequest.Assignees)
                .Set(x => x.Base, updatedPullRequest.Base)
                .Set(x => x.Body, updatedPullRequest.Body)
                .Set(x => x.Head, updatedPullRequest.Head)
                .Set(x => x.RepoName, updatedPullRequest.RepoName)
                .Set(x => x.RepositoryId, updatedPullRequest.RepositoryId);

            var result = await _pullRequestCollection.UpdateOneAsync(filter, updateDefinition);

            return result.ModifiedCount > 0 ? Maybe.From(updatedPullRequest) : Maybe.None;
        }



        public async Task<Maybe<PullRequest>> UpdateIsOpen(int index, bool isOpen)
        {
            var filter = Builders<PullRequest>.Filter.Eq(x => x.Number, index);
            var updateDefinition = Builders<PullRequest>.Update.Set(x => x.IsOpen, isOpen);

            var result = await _pullRequestCollection.UpdateOneAsync(filter, updateDefinition);
            var result1 = await _pullRequestCollection.Find(filter).FirstOrDefaultAsync();

            return result.ModifiedCount > 0 ? Maybe.From(result1) : Maybe.None;
        }





        public async Task<bool> Delete(Guid pullRequestId)
        {
            var filter = Builders<PullRequest>.Filter.Eq(x => x.Id, pullRequestId);
            var result = await _pullRequestCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<PullRequest>> SearchPullRequest(string searchString, string email, string repo)
        {
            if (searchString.Equals("") || searchString.Equals("q:")) {
                var filter1 = Builders<PullRequest>.Filter.Eq(x => x.RepoName, repo);
                var result = await _pullRequestCollection.Find(filter1).ToListAsync();
                return result is not null ? result : new List<PullRequest>();
            };
            if (searchString.Contains("q:")) return new List<PullRequest>();

            var keyValuePairs = searchString.Split(' ');
            var filterBuilder = Builders<PullRequest>.Filter;
            var filter = Builders<PullRequest>.Filter.Empty;
            SortDefinition<PullRequest>? sortDefinition = null;
            filter &= filterBuilder.Eq(x => x.RepoName, repo);
            var isSortingByUpdatedDate = 0;

            foreach (var pair in keyValuePairs)
            {
                var parts = pair.Split(':');
                if (parts.Length == 2)
                {
                    var key = parts[0];
                    var value = parts[1];

                    switch (key.ToLower())
                    {
                        case "assignee":
                            filter &= value switch
                            {
                                "" => filterBuilder.Eq(pull => pull.Assignee, null),
                                "@me" => filterBuilder.Eq(pull => pull.Assignee, email),
                                _ => filterBuilder.Eq(pull => pull.Assignee, value)
                            };
                            break;
                        case "author":
                            filter &= value switch
                            {
                                "" => filterBuilder.Eq(pull => pull.Author, null),
                                "@me" => filterBuilder.Eq(pull => pull.Author, email),
                                _ => filterBuilder.Eq(pull => pull.Author, value)
                            };
                            break;
                        case "milestone":
                            if (value.Equals(""))
                            {
                                filter &= filterBuilder.Eq(pull => pull.MilestoneId, null);
                            }
                            else
                            {
                                value = value.Replace("_", " ");
                                var (hasValue, milestoneResult) =
                                    await _milestoneRepository.GetByTitle(value);
                                if (!hasValue)
                                {
                                    return new List<PullRequest>();
                                }

                                filter &= filterBuilder.Eq(pull => pull.MilestoneId, milestoneResult.Id);
                            }
                            break;
                        case "label":
                            if (value.Equals(""))
                            {
                                filter &= filterBuilder.Or(
                                    filterBuilder.Size(pull => pull.Labels, 0),
                                    filterBuilder.Eq(pull => pull.Labels, null)
                                );
                            }
                            else
                            {
                                value = value.Replace("_", " ");
                                filter &= filterBuilder.AnyEq(pull => pull.Labels.Select(label => label.Name), value);
                            }
                            break;
                        case "is":
                            switch (value)
                            {
                                case "open":
                                    filter &= filterBuilder.Eq(pull => pull.IsOpen, true);
                                    break;
                                case "closed":
                                    filter &= filterBuilder.Eq(pull => pull.IsOpen, false);
                                    break;
                                default:
                                    return new List<PullRequest>();
                            }
                            break;
                        case "sort":
                            switch (value)
                            {
                                case "created-desc":
                                    sortDefinition = Builders<PullRequest>.Sort.Descending(
                                        pull => pull.CreatedAt);
                                    break;
                                case "created-asc":
                                    sortDefinition = Builders<PullRequest>.Sort.Ascending(
                                        pull => pull.CreatedAt);
                                    break;
                                case "comments-desc":
                                    break;
                                case "comments-asc":
                                    break;
                                case "updated-desc":
                                    isSortingByUpdatedDate = 1;
                                    break;
                                case "updated-asc":
                                    isSortingByUpdatedDate = 2;
                                    break;
                            }
                            break;
                        default:
                            return new List<PullRequest>();
                    }
                }
            }

            switch (isSortingByUpdatedDate)
            {
                case 1:
                    var pulls = await  _pullRequestCollection.Find(filter).ToListAsync();
                    pulls = pulls.OrderByDescending(
                        pull => (pull.Events ?? Array.Empty<Event>()).Max(ev => ev.DateTimeOccured)).ToList();
                    return pulls;
                case 2:
                    var pulls1 = await  _pullRequestCollection.Find(filter).ToListAsync();
                    pulls1 = pulls1.OrderBy(
                        pull => (pull.Events ?? Array.Empty<Event>()).Max(ev => ev.DateTimeOccured)).ToList();
                    return pulls1;
                default:
                    var query = _pullRequestCollection.Find(filter);
                    if (sortDefinition != null)
                    {
                        query = query.Sort(sortDefinition);
                    }
                    return await query.ToListAsync();
            }
        }

        public async Task<IEnumerable<PullRequest>> GetAllForRepo(string repo)
        {
            var filter1 = Builders<PullRequest>.Filter.Eq(x => x.RepoName, repo);
            var result = await _pullRequestCollection.Find(filter1).ToListAsync();
            return result is not null ? result : new List<PullRequest>();
        }
    }
}
