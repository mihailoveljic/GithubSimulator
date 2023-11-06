using System;

namespace GitHubSimulator.Core.Models;

public class Issue
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Assigne { get; set; }

    public Issue (
        Guid id,
        string title,
        string description,
        string assigne)
    {
        Id = id;
        Title = title;
        Description = description;
        Assigne = assigne;
    }
}
