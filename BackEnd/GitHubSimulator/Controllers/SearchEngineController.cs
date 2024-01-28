using GitHubSimulator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
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

    [HttpGet("All", Name = "SearchAll")]
    public async Task<IActionResult> GetAllPullRequestss()
    {
        try
        {
            return Ok(await cacheService.SearchAllIndexesAsync("1"));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}