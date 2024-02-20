using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;

namespace GitHubSimulator.Core.BuildingBlocks;

public abstract class Entity
{
    [Key]
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    public Guid Id { get; init; }

    protected Entity()
    {

    }

    protected Entity(Guid id)
    {
        Id = id;
    }
}