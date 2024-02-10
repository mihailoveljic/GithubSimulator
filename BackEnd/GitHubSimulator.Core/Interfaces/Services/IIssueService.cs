using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.ValueObjects;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface IIssueService
{
    Task<IEnumerable<Issue>> GetAll();
    Task<IEnumerable<Issue>> GetIssuesForMilestone(Guid milestoneId);
    Task<IEnumerable<Issue>> SearchIssues(string searchString, string email);
    Task<Maybe<Issue>> GetById(Guid id);
    Task<Issue> Insert(Issue issue);
    Task<Maybe<Issue>> Update(Issue issue);
    Task<Maybe<Issue>> UpdateIssueTitle(Guid id, string newTitle, string email);
    Task<Maybe<Issue>> UpdateIssueMilestone(Guid id, Guid? milestoneId, string email);
    Task<Maybe<Issue>> UpdateIssueAssignee(Guid id, string? assignee, string email);
    Task<Maybe<Issue>> OpenOrCloseIssue(Guid id, bool isOpen, string email);
    Task<bool> Delete(Guid id);
}
