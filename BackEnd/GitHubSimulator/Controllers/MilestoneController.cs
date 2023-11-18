using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Dtos.Milestones;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;
[ApiController]
[Route("[controller]")]
public class MilestoneController : ControllerBase
{
    private readonly IMilestoneService milestoneService;
    private readonly ILogger<MilestoneController> logger;

    public MilestoneController(
        IMilestoneService milestoneService,
        ILogger<MilestoneController> logger)
    {
        this.milestoneService = milestoneService;
        this.logger = logger;
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
            return Ok(await milestoneService.GetAllWithIssues(milestoneIds?.ids));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
