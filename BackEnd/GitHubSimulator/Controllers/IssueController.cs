using System.Security.Claims;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Issues;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.Cache;
using GitHubSimulator.Middlewares;
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
    private readonly ICacheService cacheService;
    private readonly ILabelService _labelService;
    private readonly IRepositoryService _repositoryService;
    private readonly IUserRepositoryService _userRepositoryService;

    public IssueController(
        IIssueService issueService,
        ILogger<IssueController> logger,
        IssueFactory issueFactory,
        ICacheService cacheService,
        ILabelService labelService, IRepositoryService repositoryService, IUserRepositoryService userRepositoryService)
    {
        this.issueService = issueService;
        this.logger = logger;
        this.issueFactory = issueFactory;
        this.cacheService = cacheService;
        _labelService = labelService;
        _repositoryService = repositoryService;
        _userRepositoryService = userRepositoryService;
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
        try
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;
            
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var userRepository =
                await _userRepositoryService.GetByUserNameRepositoryName(userName, dto.RepositoryName);

            if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ManageIssues"))
                return Forbid();
            
            var repository = await _repositoryService.GetByName(dto.RepositoryName);
            
            if (dto.LabelIds == null) {
                var result = await issueService.Insert(issueFactory
                    .MapToDomain(dto, userEmail, new List<Label>(), repository.Id));
                await cacheService.RemoveAllIssueDataAsync();
                return Created("Issue successfully created", result);
            }
            var newlyAddedLabels = new List<Label>();
            foreach (var labId in dto.LabelIds)
            {
                var lab = await _labelService.GetById(labId);
                newlyAddedLabels.Add(lab.Value);
            }
            await cacheService.RemoveAllIssueDataAsync();
            return Created("https://www.youtube.com/watch?v=LTyZKvIxrDg&t=3566s&ab_channel=Standuprs",
                await issueService.Insert(issueFactory
                    .MapToDomain(dto, userEmail, newlyAddedLabels, repository.Id))); 
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

    [HttpPut("updateTitle", Name = "UpdateIssueTitle")]
    public async Task<IActionResult> UpdateIssueTitle([FromBody] UpdateIssueTitleDto dto)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var issue = await issueService.GetById(dto.Id);
        var repository = await _repositoryService.GetById(issue.Value.RepositoryId);
        var userRepository = await _userRepositoryService
            .GetByUserNameRepositoryName(userName, repository.Name);
        
        if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ManageIssues"))
            return Forbid();
        
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        return (await issueService.UpdateIssueTitle(dto.Id, dto.Title, userEmail))
            .Map(issue => (IActionResult)Ok(issue))
            .GetValueOrDefault(() => NotFound());
    }
    
    [HttpPut("updateMilestone", Name = "UpdateIssueMilestone")]
    public async Task<IActionResult> UpdateIssueMilestone([FromBody] UpdateIssueMilestoneDto dto)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var issue = await issueService.GetById(dto.Id);
        var repository = await _repositoryService.GetById(issue.Value.RepositoryId);
        var userRepository = await _userRepositoryService
            .GetByUserNameRepositoryName(userName, repository.Name);
        
        if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ManageIssues"))
            return Forbid();
        
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        return (await issueService.UpdateIssueMilestone(dto.Id, dto.MilestoneId, userEmail))
            .Map(issue => (IActionResult)Ok(issue.MilestoneId))
            .GetValueOrDefault(() => NotFound());
    }
    
    [HttpPut("updateAssignee", Name = "UpdateIssueAssignee")]
    public async Task<IActionResult> UpdateIssueAssignee([FromBody] UpdateIssueAssigneeDto dto)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var issue = await issueService.GetById(dto.Id);
        var repository = await _repositoryService.GetById(issue.Value.RepositoryId);
        var userRepository = await _userRepositoryService
            .GetByUserNameRepositoryName(userName, repository.Name);
        
        if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ManageIssues"))
            return Forbid();
        
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
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var issue = await issueService.GetById(issueId);
        var repository = await _repositoryService.GetById(issue.Value.RepositoryId);
        var userRepository = await _userRepositoryService
            .GetByUserNameRepositoryName(userName, repository.Name);
        
        if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ManageIssues"))
            return Forbid();
        
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
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var issue = await issueService.GetById(dto.Id);
        var repository = await _repositoryService.GetById(issue.Value.RepositoryId);
        var userRepository = await _userRepositoryService
            .GetByUserNameRepositoryName(userName, repository.Name);
        
        if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ManageIssues"))
            return Forbid();
        
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;

        return (await issueService.OpenOrCloseIssue(dto.Id, dto.IsOpen, userEmail))
            .Map(issue
                => (IActionResult)Ok())
            .GetValueOrDefault(() => NotFound());
    }
    
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteIssue([FromQuery] Guid id)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var issue = await issueService.GetById(id);
        var repository = await _repositoryService.GetById(issue.Value.RepositoryId);
        var userRepository = await _userRepositoryService
            .GetByUserNameRepositoryName(userName, repository.Name);

        if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ManageIssues"))
            return Forbid();
        
        var response = await issueService.Delete(id);
        if (response)
        {
            await cacheService.RemoveAllIssueDataAsync();
            return NoContent();
        }

        return NotFound("Repository with provided id not found");
    }

    [HttpPost("searchIssues/{repo}", Name = "SearchIssues")]
    public async Task<IActionResult> SearchIssues(string repo, [FromBody] SearchIssuesDto dto)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var userRepository = await _userRepositoryService.GetByUserNameRepositoryName(userName, repo);
        if (!RepositoryAuthorizationMiddleware.Authorize(userRepository, "ReadIssues"))
            return Forbid();
        
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value!;
        
        var repository = await _repositoryService.GetByName(repo);
        
        return Ok(await issueService.SearchIssues(dto.SearchString, userEmail, repository.Id));
    }
}
