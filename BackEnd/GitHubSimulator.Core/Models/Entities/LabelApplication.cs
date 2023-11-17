using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Models.Entities;

sealed class LabelApplication : Event
{
    public Label Label { get; }

    public LabelApplication(
        Guid id, 
        DateTime dateTimeOccured,
        Label label) : base(id, dateTimeOccured)
    {
        Label = label;
    }
}
