using GitHubSimulator.Core.Models.AggregateRoots;

namespace GitHubSimulator.Core.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}