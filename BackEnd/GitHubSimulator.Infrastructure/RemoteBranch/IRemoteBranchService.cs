using GitHubSimulator.Infrastructure.RemoteBranch.Dtos;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemoteBranch
{
    public interface IRemoteBranchService
    {
        Task CreateBranch(string username, string repo, CreateGiteaBranchDto branchDto);
        Task DeleteBranch(string username, string repo, string branch);
        Task<GetGiteaBranchesDto> GetBranch(string username, string repo, string branch);
        Task<GetGiteaBranchesDto> UpdateBranch(string username, string repo, GetGiteaBranchesDto dto);
        Task<IEnumerable<GetGiteaBranchesDto>> GetRepoBranches(string username, string repo, int page, int limit);

    }
}
