using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Services;
using GitHubSimulator.Dtos.Issues;
using GitHubSimulator.Dtos.PullRequests;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.RemotePullRequest;
using GitHubSimulator.Infrastructure.RemotePullRequest.Dtos;
using GitHubSimulator.Infrastructure.RemoteRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PullRequestController : ControllerBase
{
	private readonly IPullRequestService pullRequestService;
	private readonly ILogger<PullRequestController> logger;
	private readonly PullRequestFactory pullRequestFactory;
	private readonly ILabelService _labelService;
    private readonly IRemotePullRequestService _remotePullRequestService;

    public PullRequestController(
		IPullRequestService pullRequestService,
		ILogger<PullRequestController> logger,
		ILabelService labelService,
		PullRequestFactory pullRequestFactory,
		IRemotePullRequestService remotePullRequestService)
	{
		this.pullRequestService = pullRequestService;
		this.logger = logger;
		this.pullRequestFactory = pullRequestFactory;
		this._labelService = labelService;
		this._remotePullRequestService = remotePullRequestService;


    }

	[HttpGet("All", Name = "GetAllPullRequests")]
	public async Task<IActionResult> GetAllPullRequestss()
	{
		try
		{
			return Ok(await pullRequestService.GetAll());
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpGet(Name = "GetPullRequestById")]
	public async Task<IActionResult> GetById([FromQuery] Guid id)
	{
		return (await pullRequestService.GetById(id))
		.Map(pullRequest => (IActionResult)Ok(pullRequest))
		.GetValueOrDefault(() =>
		{
			return NotFound();
		});
	}

	[HttpPost("{repo}")]
	public async Task<ActionResult<PullRequest>> CreatePullRequest([FromBody] InsertPullRequestDto dto, string repo)
	{
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var newlyAddedLabels = new List<Label>();
		if (dto.LabelIds != null)
		{
			foreach (var labId in dto.LabelIds)
			{
				var lab = await _labelService.GetById(labId);
				newlyAddedLabels.Add(lab.Value);
            }
		}
		var res = await _remotePullRequestService.CreatePullRequest(userName, repo, new Infrastructure.RemotePullRequest.Dtos.CreateGiteaPullRequest(dto.assignee,dto.@base, dto.body, dto.head,dto.title,null));

        return Created(
			"https://www.youtube.com/watch?v=LTyZKvIxrDg&t=3566s&ab_channel=Standuprs",
			await pullRequestService.Insert(pullRequestFactory.MapToDomain(dto, newlyAddedLabels, userName, res.Number)));
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdatePullRequest(Guid id, [FromBody] InsertPullRequestDto dto)
	{
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var newlyAddedLabels = new List<Label>();
        if (dto.LabelIds != null)
        {
            foreach (var labId in dto.LabelIds)
            {
                var lab = await _labelService.GetById(labId);
                newlyAddedLabels.Add(lab.Value);
            }
        }
        return (await pullRequestService.Update(pullRequestFactory.MapToDomain(dto, newlyAddedLabels, id, userName)))
		.Map(pullRequest => (IActionResult)Ok(pullRequest))
		.GetValueOrDefault(() =>
		{
			return NotFound();
		});
	}

	[HttpDelete]
	public async Task<ActionResult<bool>> DeletePullRequest([FromQuery] Guid id)
	{
		return Ok(await pullRequestService.Delete(id));
	}



    [HttpGet("pull/{repo}/{index}")]
    public async Task<IActionResult> GetByIndex(string repo, string index)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        try
        {
            return Ok(await _remotePullRequestService.GetPullRequest(userName, repo, index));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("pullDiff/{repo}/{index}")]
    public async Task<IActionResult> GetById(string repo, string index)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        try
        {
            return Ok(await _remotePullRequestService.GetPullRequestDiff(userName, repo, index));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("pullCommits/{repo}/{index}")]
    public async Task<IActionResult> GetPullCommits(string repo, string index)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        try
        {
            return Ok(await _remotePullRequestService.GetPullRequestCommits(userName, repo, index));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("pullMerge/{repo}/{index}")]
    public async Task<ActionResult<PullRequest>> MergePullRequest([FromBody] MergeGiteaPullRequest dto, string repo, int index)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        
        try
        {
            await _remotePullRequestService.MergePullRequest(userName, repo, index.ToString(), dto);
            await pullRequestService.UpdateIsOpen(index, false);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }

    [HttpPost("pullSearch/{repo}", Name = "SearchPullRequest")]
    public async Task<IActionResult> SearchPullRequest([FromBody] SearchIssuesDto dto, string repo)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        return Ok(await pullRequestService.SearchPullRequest(dto.SearchString, userEmail, repo));
    }

    [HttpGet("AllPull/{repo}", Name = "GetAllPullRequestsForRepo")]
    public async Task<IActionResult> GetAllPullRequestss(string repo)
    {
        try
        {
            return Ok(await pullRequestService.GetAllForRepo(repo));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
