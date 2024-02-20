using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.Entities;

namespace GitHubSimulator.Core.Models.Cache;

public class SearchResult
{
    public List<Repository> Repositories { get; set; } = new List<Repository>();
    public List<Branch> Branches { get; set; } = new List<Branch>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public List<Issue> Issues { get; set; } = new List<Issue>();
    public List<Label> Labels { get; set; } = new List<Label>();
    public List<Milestone> Milestones { get; set; } = new List<Milestone>();
}
