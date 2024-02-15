namespace GitHubSimulator.Infrastructure.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public Guid TaskId { get; set; }
}
