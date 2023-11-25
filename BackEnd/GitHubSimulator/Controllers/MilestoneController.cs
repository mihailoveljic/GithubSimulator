﻿using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Milestones;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;
[ApiController]
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

    [HttpGet(Name = "GetAllMilestones")]
    public async Task<IActionResult> GetAllMilestones()
    {
        try
        {
            return Ok(await milestoneService.GetAll());
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("withIssues", Name = "GetMilestonesWithIssues")]
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
        return Created("https://www.youtube.com/watch?v=LTyZKvIxrDg&t=3566s&ab_channel=Standuprs", await milestoneService.Insert(milestoneFactory.MapToDomain(dto)));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMilestone([FromBody] UpdateMilestoneDto dto)
    {
        return (await milestoneService.Update(milestoneFactory.MapToDomain(dto)))
        .Map(milestone => (IActionResult)Ok(milestone))
        .GetValueOrDefault(() => {
            return NotFound();
        });
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteMilestone([FromQuery] Guid id)
    {
        return Ok(await milestoneService.Delete(id));
    }
}
