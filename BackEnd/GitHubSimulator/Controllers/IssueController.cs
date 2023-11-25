﻿using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Issues;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
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

    [HttpGet(Name = "GetAllIssues")]
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
        .GetValueOrDefault(() => {
            return NotFound();
        });
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteIssue([FromQuery] Guid id)
    {
        return Ok(await issueService.Delete(id));
    }
}
