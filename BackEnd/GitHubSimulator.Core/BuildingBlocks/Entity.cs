namespace GitHubSimulator.Core.BuildingBlocks;

public abstract class Entity
{
    public Guid Id { get; init; }

    protected Entity()
    {

    }

    protected Entity(Guid id)
    {
        Id = id;
    }
}