using GitHubSimulator.Core.BuildingBlocks;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;

namespace GitHubSimulator.Core.Models.Abstractions;

public abstract class Task : Entity
{
    public IEnumerable<Event>? Events { get; init; }
    public IEnumerable<Label>? Labels { get; init; }
    public TaskType TaskType { get; init; }

    protected Task(
        Guid id,
        TaskType taskType,
        IEnumerable<Event>? events = null,
        IEnumerable<Label>? labels = null
        ) : base(id)
    {
        Events = events;
        Labels = labels;
        TaskType = taskType;
    }
}
