using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Infrastructure.Models;
public class Milestone
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public State State { get; set; }
    public Guid RepositoryId { get; set; }
}
