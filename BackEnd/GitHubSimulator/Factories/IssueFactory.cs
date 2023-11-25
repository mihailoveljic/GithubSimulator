using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Factories;

public class IssueFactory
{
    public Issue MapToDomain(InsertIssueDto dto) =>
        Issue.Create(dto.Title,
                     dto.Description,
                     Core.Models.ValueObjects.Mail.Create(dto.Assigne.Email),
                     dto.RepositoryId,
                     dto.MilestoneId,
                     dto.Events);

    public Issue MapToDomain(UpdateIssueDto dto) =>
        Issue.Create(dto.Title,
                     dto.Description,
                     Core.Models.ValueObjects.Mail.Create(dto.Assigne.Email),
                     dto.RepositoryId,
                     dto.MilestoneId,
                     dto.Events,
                     dto.Id);
}
