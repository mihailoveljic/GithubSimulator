using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Services;
using GitHubSimulator.Dtos.Milestones;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class MilestoneController : ControllerBase
{
    private readonly IMilestoneService milestoneService;
    private readonly ILogger<MilestoneController> logger;
    private readonly MilestoneFactory milestoneFactory;

    public MilestoneController(
        IMilestoneService milestoneService,
        ILogger<MilestoneController> logger,
        MilestoneFactory milestoneFactory)
    {
        this.milestoneService = milestoneService;
        this.logger = logger;
        this.milestoneFactory = milestoneFactory;
    }

    [HttpGet("All", Name = "GetAllMilestones")]
    public async Task<IActionResult> GetAllMilestones()
    {
        try
        {
            return Ok(await milestoneService.GetAll());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet(Name = "GetMilestoneById")]
    public async Task<IActionResult> GetById([FromQuery] Guid id)
    {
        return (await milestoneService.GetById(id))
        .Map(pullRequest => (IActionResult)Ok(pullRequest))
        .GetValueOrDefault(() => {
            return NotFound();
        });
    }

    [HttpGet("WithIssues", Name = "GetMilestonesWithIssues")]
    public async Task<IActionResult> GetAllMilestonesWithIssues([FromQuery] MilestoneIds milestoneIds)
    {
        try
        {
            return Ok(await milestoneService.GetAllWithIssues(milestoneIds?.Ids));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<Milestone>> CreateMilestone([FromBody] InsertMilestoneDto dto)
    {
        try
        {
            var result = await milestoneService.Insert(milestoneFactory.MapToDomain(dto));
            return Created("Milestone successfully created", result);
        }
        catch (FluentValidation.ValidationException ve)
        {
            return BadRequest("Fluent validation error: " + ve.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Interval Server Error" + e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMilestone([FromBody] UpdateMilestoneDto dto)
    {
        return (await milestoneService.Update(milestoneFactory.MapToDomain(dto)))
        .Map(milestone => (IActionResult)Ok(milestone))
        .GetValueOrDefault(() =>
        {
            return NotFound();
        });
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteMilestone([FromQuery] Guid id)
    {
        return Ok(await milestoneService.Delete(id));
    }
}
