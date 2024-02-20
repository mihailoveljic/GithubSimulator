using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Branches;

namespace GitHubSimulator.Factories;

public class BranchFactory
{
    public Branch MapToDomain(InsertBranchDto dto) =>
        Branch.Create(
            dto.Name,
            dto.RepositoryId,
            dto.IssueId
        );

    public Branch MapToDomain(UpdateBranchDto dto) => 
        Branch.Create(
            dto.Name,
            Guid.Empty,
            dto.IssueId,
            null,
            dto.Id
        );
}