using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Services;
using GitHubSimulator.Dtos.Issues;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class IssueController : ControllerBase
{
    private readonly IIssueService issueService;
    private readonly ILogger<IssueController> logger;
    private readonly IssueFactory issueFactory;

    public IssueController(
        IIssueService issueService,
        ILogger<IssueController> logger,
        IssueFactory issueFactory)
    {
        this.issueService = issueService;
        this.logger = logger;
        this.issueFactory = issueFactory;
    }
    
    [HttpGet("All", Name = "GetAllIssues")]
    public async Task<IActionResult> GetAllIssues()
    {
        try
        {
            return Ok(await issueService.GetAll());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet(Name = "GetIssueById")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        return (await issueService.GetById(id))
        .Map(pullRequest => (IActionResult)Ok(pullRequest))
        .GetValueOrDefault(() => {
            return NotFound();
        });
    }

    [HttpPost]
    public async Task<ActionResult<Issue>> CreateIssue([FromBody] InsertIssueDto dto)
    {
        return Created("https://www.youtube.com/watch?v=LTyZKvIxrDg&t=3566s&ab_channel=Standuprs", await issueService.Insert(issueFactory.MapToDomain(dto)));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateIssue([FromBody] UpdateIssueDto dto)
    {
        return (await issueService.Update(issueFactory.MapToDomain(dto)))
        .Map(issue => (IActionResult)Ok(issue))
        .GetValueOrDefault(() =>
        {
            return NotFound();
        });
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteIssue([FromQuery] Guid id)
    {
        return Ok(await issueService.Delete(id));
    }
}
