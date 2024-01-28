using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Infrastructure.Models;

public class Branch
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid RepositoryId { get; set; }
    public Guid? IssueId { get; set; }
    public IEnumerable<Commit>? Commits { get; set; }
}
