using GitHubSimulator.Core.BuildingBlocks;

namespace GitHubSimulator.Core.Models.Entities;

sealed class PullRequest : Entity, Abstractions.Task
{
    public Branch Source { get; }
    public Branch Target { get; }

    public PullRequest(
        Branch source, 
        Branch target, 
        Guid id) : base(id)
    {
        Source = source;
        Target = target;
    }
}
