using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IIssueService
{
    Task<IEnumerable<Issue>> GetAll();
    Task<Maybe<Issue>> GetById(Guid id);
    Task<Issue> Insert(Issue issue);
    Task<Maybe<Issue>> Update(Issue issue);
    Task<Maybe<Issue>> UpdateIssueTitle(Guid id, string newTitle);
    Task<Maybe<Issue>> UpdateIssueMilestone(Guid id, Guid? milestoneId);
    Task<Maybe<Issue>> UpdateIssueAssignee(Guid id, string? assignee);
    Task<bool> Delete(Guid id);
}
