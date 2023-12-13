namespace GitHubSimulator.Infrastructure.Configuration
{
    public sealed class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Key { get; set; } = null!;
    }
}
