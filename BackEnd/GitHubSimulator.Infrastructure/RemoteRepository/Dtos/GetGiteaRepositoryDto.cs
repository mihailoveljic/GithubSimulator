using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Infrastructure.RemoteRepository.Dtos
{
    public class GetGiteaRepositoryDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public bool Private { get; init; }
        public Visibility Visibility { get; init; }
        public string Gitignores { get; init; }
        public string License { get; init; }
        public string Default_Branch { get; init; }
        public int Stars_Count { get; init; }
        public int Forks_Count { get; init; }
        public int Watchers_Count { get; init; }
        public DateTime Created_At { get; init; }
        public DateTime Updated_At { get; init; }
        public bool Archived { get; init; }
        public int Id { get; init; }
        
        public GetGiteaRepositoryDto(string name, string description, bool @private, string gitignores, string license, string default_Branch, int stars_Count, int forks_Count, int watchers_Count, DateTime created_At, DateTime updated_At, bool archived, int id)
        {
            Name = name;
            Description = description;
            Private = @private;
            Visibility = Private ? Visibility.Private : Visibility.Public;
            Gitignores = gitignores;
            License = license;
            Default_Branch = default_Branch;
            Stars_Count = stars_Count;
            Forks_Count = forks_Count;
            Watchers_Count = watchers_Count;
            Created_At = created_At;
            Updated_At = updated_At;
            Archived = archived;
            Id = id;
        }
    }
}
