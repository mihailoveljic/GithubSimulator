using System.Security.Claims;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
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
            .Map(issue => (IActionResult)Ok(issue))
            .GetValueOrDefault(() => { return NotFound(); });
    }

    [HttpGet("getIssuesForMilestone", Name = "GetIssuesForMilestone")]
    public async Task<IActionResult> GetIssuesForMilestone([FromQuery] Guid milestoneId)
    {
        try
        {
            return Ok(await issueService.GetIssuesForMilestone(milestoneId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Issue>> CreateIssue([FromBody] InsertIssueDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;
        return Created("https://www.youtube.com/watch?v=LTyZKvIxrDg&t=3566s&ab_channel=Standuprs",
            await issueService.Insert(issueFactory.MapToDomain(dto, userEmail)));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateIssue([FromBody] UpdateIssueDto dto)
    {
        return (await issueService.Update(issueFactory.MapToDomain(dto)))
            .Map(issue => (IActionResult)Ok(issue))
            .GetValueOrDefault(() => { return NotFound(); });
    }

    [HttpPut("updateTitle", Name = "UpdateIssueTitle")]
    public async Task<IActionResult> UpdateIssueTitle([FromBody] UpdateIssueTitleDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        return (await issueService.UpdateIssueTitle(dto.Id, dto.Title, userEmail))
            .Map(issue => (IActionResult)Ok(issue))
            .GetValueOrDefault(() => NotFound());
    }

    [HttpPut("updateMilestone", Name = "UpdateIssueMilestone")]
    public async Task<IActionResult> UpdateIssueMilestone([FromBody] UpdateIssueMilestoneDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        return (await issueService.UpdateIssueMilestone(dto.Id, dto.MilestoneId, userEmail))
            .Map(issue => (IActionResult)Ok(issue.MilestoneId))
            .GetValueOrDefault(() => NotFound());
    }

    [HttpPut("updateAssignee", Name = "UpdateIssueAssignee")]
    public async Task<IActionResult> UpdateIssueAssignee([FromBody] UpdateIssueAssigneeDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        if (dto.Assignee != null)
        {
            return (await issueService.UpdateIssueAssignee(dto.Id, dto.Assignee.Email, userEmail))
                .Map(issue => (IActionResult)Ok())
                .GetValueOrDefault(() => NotFound());
        }

        return (await issueService.UpdateIssueAssignee(dto.Id, null, userEmail))
            .Map(issue => (IActionResult)Ok())
            .GetValueOrDefault(() => NotFound());
    }

    [HttpPut("updateLabels", Name = "UpdateIssueLabels")]
    public async Task<IActionResult> UpdateIssueLabels([FromQuery] Guid issueId,
        [FromBody] UpdateIssueLabelsDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;
        var labelIds = dto.LabelIds.ToList();

        try
        {
            return Ok(await issueService.UpdateIssueLabels(issueId, userEmail, labelIds));
        } 
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("openOrClose", Name = "OpenOrCloseIssue")]
    public async Task<IActionResult> OpenOrCloseIssue([FromBody] OpenOrCloseIssueDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        return (await issueService.OpenOrCloseIssue(dto.Id, dto.IsOpen, userEmail))
            .Map(issue
                => (IActionResult)Ok())
            .GetValueOrDefault(() => NotFound());
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteIssue([FromQuery] Guid id)
    {
        return Ok(await issueService.Delete(id));
    }

    [HttpPost("searchIssues", Name = "SearchIssues")]
    public async Task<IActionResult> SearchIssues([FromBody] SearchIssuesDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;
        
        return Ok(await issueService.SearchIssues(dto.SearchString, userEmail));
    }
}
