using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.PullRequests;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PullRequestController : ControllerBase
{
	private readonly IPullRequestService pullRequestService;
	private readonly ILogger<PullRequestController> logger;
	private readonly PullRequestFactory pullRequestFactory;

	public PullRequestController(
		IPullRequestService pullRequestService,
		ILogger<PullRequestController> logger,
		PullRequestFactory pullRequestFactory)
	{
		this.pullRequestService = pullRequestService;
		this.logger = logger;
		this.pullRequestFactory = pullRequestFactory;
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

	[HttpPost]
	public async Task<ActionResult<PullRequest>> CreatePullRequest([FromBody] InsertPullRequestDto dto)
	{
		return Created(
			"https://www.youtube.com/watch?v=LTyZKvIxrDg&t=3566s&ab_channel=Standuprs",
			await pullRequestService.Insert(pullRequestFactory.MapToDomain(dto)));
	}

	[HttpPut]
	public async Task<IActionResult> UpdatePullRequest([FromBody] UpdatePullRequestDto dto)
	{
		return (await pullRequestService.Update(pullRequestFactory.MapToDomain(dto)))
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
}
