using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Milestones;

namespace GitHubSimulator.Factories;

public class MilestoneFactory
{
    public Milestone MapToDomain(InsertMilestoneDto dto, Guid repositoryId) =>
        Milestone.Create(
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.State,
            repositoryId);

    public Milestone MapToDomain(UpdateMilestoneDto dto) =>
        Milestone.Create(
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.State,
            dto.RepositoryId,
            dto.Id);
}
