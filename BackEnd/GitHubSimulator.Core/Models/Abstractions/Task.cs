using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Abstractions;

public abstract class Task : Entity
{
    public IEnumerable<Event>? Events { get; init; }
    public TaskType TaskType { get; init; }

    protected Task(
        Guid id,
        TaskType taskType,
        IEnumerable<Event>? events = null) : base(id)
    {
        Events = events;
        TaskType = taskType;
    }
}
