using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Infrastructure.Models;
public class Issue
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string AssigneeEmail { get; set; }
    public Guid RepositoryId { get; set; }
    public Guid? MilestoneId { get; set; }
}
