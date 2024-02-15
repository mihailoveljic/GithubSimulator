namespace GitHubSimulator.Infrastructure.Configuration
{
    public sealed class RemoteRepositorySettings
    {
        public const string SectionName = "RemoteRepositorySettings";
        public string BaseURL { get; set; } = null!;
        public string AdminAccessToken { get; set; } = null!;
    }
}
