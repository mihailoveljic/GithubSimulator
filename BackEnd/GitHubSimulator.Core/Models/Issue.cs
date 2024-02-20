using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;
namespace GitHubSimulator.Core.Models;

public class Issue
{
    [Key]
    [BsonId(IdGenerator = typeof(GuidGenerator))]
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
