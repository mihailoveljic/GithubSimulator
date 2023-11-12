using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IIssueService issueService;
    private readonly ICacheService cacheService;
    private readonly ILogger<WeatherForecastController> logger;

    public WeatherForecastController(
        IIssueService issueService,
        ICacheService cacheService,
        ILogger<WeatherForecastController> logger)
    {
        this.issueService = issueService;
        this.cacheService = cacheService;
        this.logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("test",Name = "GetAllIssues")]
    public async Task<IActionResult> GetAllIssues()
    {
        var cacheData = cacheService.GetData<IEnumerable<Issue>>("issues");

        if(cacheData != null && cacheData.Count()>0)
        {
            logger.LogInformation("Imam Ke$");
            return Ok(cacheData);
        }

        cacheData = await issueService.GetAll();
        var expiryTime = DateTimeOffset.Now.AddSeconds(30);
        cacheService.SetData("issues", cacheData, expiryTime);

        logger.LogInformation("Nemam Ke$");
        return Ok(cacheData);
    }
}
