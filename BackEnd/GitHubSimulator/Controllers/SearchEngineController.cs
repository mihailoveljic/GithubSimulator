using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Dtos.Milestones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
[Route("[controller]")]
public class SearchEngineController : ControllerBase
{
    private readonly ICacheService cacheService;
    private readonly ILogger<SearchEngineController> logger;

    public SearchEngineController(
        ICacheService cacheService,
        ILogger<SearchEngineController> logger)
    {
        this.cacheService = cacheService;
        this.logger = logger;
    }

    [HttpGet(Name = "SearchAll")]
    public async Task<IActionResult> SearchAll([FromQuery] string searchTerm)
    {
        try
        {
            return Ok(await cacheService.SearchAllIndexesAsync(searchTerm));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}