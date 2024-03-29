﻿namespace GitHubSimulator.Infrastructure.Configuration;

public class DatabaseSettings
{
    public const string SectionName = "MongoDatabaseSettings";
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string IssueCollectionName { get; set; } = null!;
    public string MilestoneCollectionName { get; set; } = null!;
    public string UserCollectionName { get; set; } = null!;
    public string RepositoryCollectionName { get; set; } = null!;
    public string LabelCollectionName { get; set; } = null!;
    public string BranchCollectionName { get; set; } = null!;
    public string PullRequestCollectionName { get; set; } = null!;
    public string CommentCollectionName { get; set; } = null!;
    public string UserRepositoryCollectionName { get; set; } = null!;
}
