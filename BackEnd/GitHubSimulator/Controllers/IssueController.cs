using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Services;
using GitHubSimulator.Dtos.Issues;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
//[Authorize]
[Route("[controller]")]
public class IssueController : ControllerBase
{
    private readonly IIssueService issueService;
    private readonly ILogger<IssueController> logger;
    private readonly IssueFactory issueFactory;
    private readonly ICacheService cacheService;

    public IssueController(
        IIssueService issueService,
        ILogger<IssueController> logger,
        IssueFactory issueFactory,
        ICacheService cacheService)
    {
        this.issueService = issueService;
        this.logger = logger;
        this.issueFactory = issueFactory;
        this.cacheService = cacheService;
    }
    
    [HttpGet("All", Name = "GetAllIssues")]
    public async Task<IActionResult> GetAllIssues()
    {
        var cachedIssues = await cacheService.GetAllIssuesAsync();

        if (cachedIssues != null && cachedIssues.Any())
        {
            logger.LogInformation("Iz kesa");
            return Ok(cachedIssues);
        }

        try
        {
            var issues = await issueService.GetAll();
            if (issues.Any())
            {
                foreach (var issue in issues)
                {
                    logger.LogInformation("Cuvam u kes");
                    await cacheService.SetIssueData(issue, DateTimeOffset.UtcNow.AddHours(2));
                }
            }
            logger.LogInformation("Bez kesa");
            return Ok(issues);
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
        try
        {
            var result = await issueService.Insert(issueFactory.MapToDomain(dto));
            // Invalidate or update cache as necessary
            await cacheService.RemoveAllIssueDataAsync(); // Invalidate cache
            return Created("Issue successfully created", result);
        }
        catch (FluentValidation.ValidationException ve)
        {
            return BadRequest("Fluent validation error: " + ve.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateIssue([FromBody] UpdateIssueDto dto)
    {
        var updateResult = await issueService.Update(issueFactory.MapToDomain(dto));

        if (updateResult.HasValue)
        {
            await cacheService.RemoveAllIssueDataAsync(); // Invalidate cache asynchronously
            return Ok(updateResult.Value); // Directly return Ok result if update is successful
        }
        else
        {
            return NotFound(); // Return NotFound if the updateResult does not have a value
        }
    }



    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteIssue([FromQuery] Guid id)
    {
        var response = await issueService.Delete(id);
        if (response)
        {
            await cacheService.RemoveAllIssueDataAsync();
            return NoContent();
        }

        return NotFound("Repository with provided id not found");
    }
}
