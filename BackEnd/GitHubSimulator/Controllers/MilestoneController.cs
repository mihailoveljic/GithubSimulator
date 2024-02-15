using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Milestones;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.Cache;
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
    private readonly ICacheService cacheService;

    public MilestoneController(
        IMilestoneService milestoneService,
        ILogger<MilestoneController> logger,
        MilestoneFactory milestoneFactory,
        ICacheService cacheService)
    {
        this.milestoneService = milestoneService;
        this.logger = logger;
        this.milestoneFactory = milestoneFactory;
        this.cacheService = cacheService;
    }

    [HttpGet("All", Name = "GetAllMilestones")]
    public async Task<IActionResult> GetAllMilestones()
    {
        try
        {
            var cachedMilestones = await cacheService.GetAllMilestonesAsync();
            if (cachedMilestones != null && cachedMilestones.Any())
            {
                logger.LogInformation("Milestones retrieved from cache");
                return Ok(cachedMilestones);
            }

            var milestones = await milestoneService.GetAll();
            if (milestones.Any())
            {
                foreach (var milestone in milestones)
                {
                    await cacheService.SetMilestoneData(milestone, DateTimeOffset.UtcNow.AddHours(2));
                }
            }
            logger.LogInformation("Milestones retrieved from the service and cached");
            return Ok(milestones);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve milestones");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("getProgress", Name = "GetMilestoneProgress")]
    public async Task<IActionResult> GetMilestoneProgress([FromQuery] Guid milestoneId)
    {
        try
        {
            return Ok(await milestoneService.GetMilestoneProgress(milestoneId)); 
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("AllForRepo", Name = "GetAllMilestonesForRepository")]
    public async Task<IActionResult> GetAllMilestonesForRepository([FromQuery] Guid repoId)
    {
        try
        {
            var result = await milestoneService.GetAllMilestonesForRepository(repoId);
            if (!result.Any())
            {
                return NotFound("This repository has no milestones");
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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
            await cacheService.RemoveAllMilestoneDataAsync();
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

    [HttpPut("reopenOrClose", Name = "ReopenOrCloseMilestone")]
    public async Task<IActionResult> ReopenOrCloseMilestone([FromBody] ReopenOrCloseMilestoneDto dto)
    {
        return (await milestoneService.ReopenOrCloseMilestone(dto.Id, dto.State))
            .Map(milestone => (IActionResult)Ok(milestone))
            .GetValueOrDefault(() => NotFound());
    }

    [HttpPost("getOpenOrClosed", Name = "GetOpenOrClosedMilestones")]
    public async Task<IActionResult> GetOpenOrClosedMilestones([FromBody] GetOpenOrClosedMilestonesDto dto)
    {
        try
        {
            return Ok(await milestoneService.GetOpenOrClosedMilestones(dto.Id, dto.State));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMilestone([FromBody] UpdateMilestoneDto dto)
    {
        var updateResult = await milestoneService.Update(milestoneFactory.MapToDomain(dto));

        if (updateResult.HasValue)
        {
            await cacheService.RemoveAllMilestoneDataAsync(); // Invalidate cache after update
            return Ok(updateResult.Value); // Directly return the result if available
        }
        else
        {
            return NotFound(); // Return NotFound if the update result has no value
        }
    }



    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteMilestone([FromQuery] Guid id)
    {
        var result = await milestoneService.Delete(id);
        if (result)
        {
            await cacheService.RemoveAllMilestoneDataAsync();
            return Ok("Milestone deleted successfully");
        }
        return NotFound("Milestone with the provided ID not found");
    }

}
