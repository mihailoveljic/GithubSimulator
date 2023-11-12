namespace GitHubSimulator.Infrastructure.Configuration;

public class DatabaseSettings
{
    public const string SectionName = "MongoDatabaseSettings";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string IssueCollectionName { get; set; } = null!;
}
