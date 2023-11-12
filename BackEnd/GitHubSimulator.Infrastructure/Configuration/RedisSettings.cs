namespace GitHubSimulator.Infrastructure.Configuration;

public class RedisSettings
{
    public const string SectionName = "RedisSettings";
    public string URL { get; set; } = null!;
    public string Password { get; set; } = null!;
}
