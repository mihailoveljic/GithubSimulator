using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Dtos.Milestones;

public class MilestoneWithIssues
{
    public Guid Id { get; }
    public string Title { get; }
    public string? Description { get; }
    public DateTime? DueDate { get; }
    public State State { get; }
    public Guid RepositoryId { get; }
    public IEnumerable<Issue> Issues { get; }

    public MilestoneWithIssues(
        Guid id,
        string title,
        string? description,
        DateTime? dueDate,
        State state,
        Guid repositoryId,
        IEnumerable<Issue> issues)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        State = state;
        RepositoryId = repositoryId;
        Issues = issues;
    }
}
