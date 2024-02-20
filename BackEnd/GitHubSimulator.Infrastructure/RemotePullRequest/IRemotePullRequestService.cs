using GitHubSimulator.Infrastructure.RemotePullRequest.Dtos;
using GitHubSimulator.Infrastructure.RemoteRepository.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubSimulator.Infrastructure.RemotePullRequest
{
    public interface IRemotePullRequestService
    {

        Task<GiteaPullRequestDto> CreatePullRequest(string username, string repo, CreateGiteaPullRequest pullRequestDto);
        Task<string> GetPullRequestDiff(string username, string repo, string index);
        Task<string> CommitDiff(string username, string repo, string sha);
        Task<IEnumerable<GiteaCommitDto>> GetPullRequestCommits(string username, string repo, string index);
        Task<GiteaPullRequestDto> GetPullRequest(string username, string repo, string index);
        Task MergePullRequest(string username, string repo, string index, MergeGiteaPullRequest pullRequestDto);
        //Task<IEnumerable<GetGiteaRepositoryDto>> GetRepoPullRequest(string username, int page, int limit);

    }
}
