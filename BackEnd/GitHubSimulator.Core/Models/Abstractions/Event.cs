using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Abstractions;

public class Event : Entity
{
    public DateTime DateTimeOccured { get; }
    public EventType EventType { get; }

    public Event(
        Guid id, 
        DateTime dateTimeOccured,
        EventType eventType) : base(id)
    {
        DateTimeOccured = dateTimeOccured;
        EventType = eventType;
    }
}
