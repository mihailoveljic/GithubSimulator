namespace GitHubSimulator.Infrastructure.Factories;
public static class BranchFactory
{
    public static Core.Models.Entities.Branch MapToDomain(Models.Branch branch) =>
        Core.Models.Entities.Branch.Create(
            branch.Name,
            branch.RepositoryId,
            branch.IssueId,
            null,
            branch.Id);
}
