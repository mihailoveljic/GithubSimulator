using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Abstractions;

public class Event : Entity
{
    public DateTime DateTimeOccured { get; init; }
    public EventType EventType { get; init; }

    public Event(
        Guid id, 
        DateTime dateTimeOccured,
        EventType eventType) : base(id)
    {
        DateTimeOccured = dateTimeOccured;
        EventType = eventType;
    }
}
