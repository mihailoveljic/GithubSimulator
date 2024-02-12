﻿using GitHubSimulator.Core.Models.Abstractions;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Issues;

namespace GitHubSimulator.Factories;

public class IssueFactory
{
    public Issue MapToDomain(InsertIssueDto dto, string authorEmail) =>
        Issue.Create(dto.Title,
            dto.Description,
            Core.Models.ValueObjects.Mail.Create(dto.Assigne.Email),
            Core.Models.ValueObjects.Mail.Create(authorEmail),
            dto.RepositoryId,
            dto.MilestoneId,
            dto.Events ?? new List<Event>(),
            dto.Labels ?? new List<Label>());

    public Issue MapToDomain(UpdateIssueDto dto) =>
        Issue.Create(dto.Title,
                     dto.Description,
                     Core.Models.ValueObjects.Mail.Create(dto.Assigne.Email),
                     Core.Models.ValueObjects.Mail.Create(dto.Assigne.Email),
                     dto.RepositoryId,
                     dto.MilestoneId,
                     dto.Events,
                     null,
                     dto.Id);
}
