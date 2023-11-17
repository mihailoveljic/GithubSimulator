using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.Abstractions;

public abstract class Event : Entity
{
    public DateTime DateTimeOccured { get; init; }

    protected Event(
        Guid id, 
        DateTime dateTimeOccured) : base(id)
    {
        DateTimeOccured = dateTimeOccured;
    }
}
