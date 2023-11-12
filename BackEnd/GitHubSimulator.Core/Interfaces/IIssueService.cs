using GitHubSimulator.Core.Models;

namespace GitHubSimulator.Core.Interfaces;

public interface IIssueService
{
    Task<IEnumerable<Issue>> GetAll();
}
