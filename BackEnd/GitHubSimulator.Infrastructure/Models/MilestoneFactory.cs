namespace GitHubSimulator.Infrastructure.Models;

public static class MilestoneFactory
{
    public static Core.Models.Entities.Milestone MapToDomain(Milestone milestone) =>
        Core.Models.Entities.Milestone.Create(
            milestone.Title,
            milestone.Description,
            (DateTime)(milestone.DueDate is null ? DateTime.MinValue : milestone.DueDate),
            milestone.State,
            milestone.RepositoryId,
            milestone.Id);
}
