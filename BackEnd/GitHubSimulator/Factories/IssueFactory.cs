using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Factories;

public class IssueFactory
{
    public Issue MapToDomain(InsertIssueDto dto, string authorEmail, IEnumerable<Label> newLabels)
        => Issue.Create(dto.Title,
            dto.Description,
            Core.Models.ValueObjects.Mail.Create(dto.Assignee.Email),
            Core.Models.ValueObjects.Mail.Create(authorEmail),
            dto.RepositoryId,
            dto.MilestoneId,
            dto.Events ?? new List<Event>(),
            newLabels); 

    public Issue MapToDomain(UpdateIssueDto dto) =>
        Issue.Create(dto.Title,
                     dto.Description,
                     Core.Models.ValueObjects.Mail.Create(dto.Assignee.Email),
                     Core.Models.ValueObjects.Mail.Create(dto.Assignee.Email),
                     dto.RepositoryId,
                     dto.MilestoneId,
                     dto.Events,
                     null,
                     dto.Id);
}
