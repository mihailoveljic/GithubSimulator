using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Infrastructure.Models;

public class Repository
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Visibility Visibility { get; set; }
}
