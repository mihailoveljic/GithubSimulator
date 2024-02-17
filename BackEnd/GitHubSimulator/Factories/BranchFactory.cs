using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Branches;
using GitHubSimulator.Dtos.Repositories;
using GitHubSimulator.Infrastructure.RemoteBranch.Dtos;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;

namespace GitHubSimulator.Factories;

public class BranchFactory
{
    public Branch MapToDomain(InsertBranchDto dto) =>
        Branch.Create(
            dto.New_Branch_Name,
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

    public CreateGiteaBranchDto MapToGiteaDto(InsertBranchDto dto) =>
        new(dto.New_Branch_Name, dto.Old_Branch_Name, dto.Old_Ref_Name);
}