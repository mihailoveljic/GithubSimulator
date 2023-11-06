using GitHubSimulator.Core.Models;

namespace GitHubSimulator.Core.Interfaces;

public interface IIssueRepository
{
    Task<IEnumerable<Issue>> GetAll();
}
