using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Dtos.Repositories;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.RemoteRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class RepositoryController : ControllerBase
{
    private readonly IRepositoryService _repositoryService;
    private readonly IRemoteRepositoryService _remoteRepositoryService;
    private readonly RepositoryFactory _repositoryFactory;
    private readonly ICacheService _cacheService;

    public RepositoryController(IRepositoryService repositoryService, IRemoteRepositoryService remoteRepositoryService, RepositoryFactory repositoryFactory, ICacheService cacheService)
    {
        _repositoryService = repositoryService;
        _remoteRepositoryService = remoteRepositoryService;
        _repositoryFactory = repositoryFactory;
        _cacheService = cacheService;
    }

    [HttpGet("{owner}")]
    public async Task<IActionResult> GetUserRepositories(string owner, [FromQuery] int page, [FromQuery] int limit)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var response = await _remoteRepositoryService.GetUserRepositories(owner, page, limit);

            if(userName == owner)
            {
                return Ok(response);
            }
 
            response = response.Where(response => response.Private == false);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("All", Name = "GetAllRepositories")]
    public async Task<IActionResult> GetAllRepositories()
    {
        var cachedRepositories = await _cacheService.GetAllRepositoriesAsync();

        if (cachedRepositories != null && cachedRepositories.Any())
        {
            Console.Write("Iz kesa");
            return Ok(cachedRepositories);
        }

        try
        {
            var repositories = await _repositoryService.GetAll();
            if (repositories.Any())
            {
                foreach (var repo in repositories)
                {
                    Console.Write("Cuvam u kes");
                    Console.WriteLine(repo.Name);
                    _cacheService.SetRepositoryData(repo, DateTimeOffset.UtcNow.AddHours(2));
                }
            }
            Console.Write("Bez kesa");
            return Ok(repositories);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}", Name = "GetRepositoryById")]
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    public async Task<IActionResult> GetRepositoryById(Guid id)
    {
        var response = await _repositoryService.GetById(id);
        if (response is null)
        {
            return NotFound("A repository with the provided ID not found");
        }

        return Ok(response);
    }

    [HttpGet("{owner}/{repositoryName}")]
    public async Task<IActionResult> GetRepository(string owner, string repositoryName)
    {
        var response = await _remoteRepositoryService.GetRepository(owner, repositoryName);
        if (response is null)
        {
            return NotFound("A repository with the provided name not found");
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRepository([FromBody] InsertRepositoryDto dto)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService.CreateRepository(userName, _repositoryFactory.MapToGiteaDto(dto));
            var result = await _repositoryService.Insert(_repositoryFactory.MapToDomain(dto));
            await _cacheService.RemoveAllRepositoryDataAsync(); // Invalidate cache

            return Created("Repository successfully created", result);
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
    public async Task<IActionResult> UpdateRepository([FromBody] UpdateRepositoryDto dto)
    {
        var response = await _repositoryService.Update(_repositoryFactory.MapToDomain(dto));
        if (response.Equals(Maybe<Repository>.None))
        {
            return NotFound("A repository with the provided ID not found");
        }

        return Ok(response.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRepository([FromRoute] Guid id)
    {
        var response = await _repositoryService.Delete(id);
        if (response)
        {
            await _cacheService.RemoveAllRepositoryDataAsync();
            return NoContent();
        }

        return NotFound("Repository with provided id not found");
    }

    [HttpGet("{owner}/{repositoryName}/content/{*filePath}")]
    public async Task<IActionResult> GetRepositoryContent(string owner, string repositoryName, string filePath, [FromQuery] string branchName = "main")
    {
        try
        {
            if (filePath == "_home_")
            {
                filePath = ".";
            }
            var response = await _remoteRepositoryService.GetRepositoryContent(owner, repositoryName, filePath, branchName);
            if (response is null)
            {
                return NotFound("Repository is empty");
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{repositoryName}/content/{*filePath}")]
    public async Task<IActionResult> GetRepositoryContent(string repositoryName, string filePath, [FromQuery] string branchName = "main")
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            if (filePath == "_home_")
            {
                filePath = ".";
            }
            var response = await _remoteRepositoryService.GetRepositoryContent(userName, repositoryName, filePath, branchName);
            if (response is null)
            {
                return NotFound("Repository is empty");
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("fork/{owner}/{repositoryName}")]
    public async Task<IActionResult> ForkRepository(string owner, string repositoryName, [FromQuery] string forkName)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService.ForkRepo(userName, owner, repositoryName, forkName);
            return Ok("Repository successfully forked");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("watch/{owner}/{repositoryName}")]
    public async Task<IActionResult> WatchRepository(string owner, string repositoryName)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService.WatchRepo(userName, owner, repositoryName);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("watch/{owner}/{repositoryName}")]
    public async Task<IActionResult> UnwatchRepository(string owner, string repositoryName)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService.UnwatchRepo(userName, owner, repositoryName);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("watch/{owner}/{repositoryName}")]
    public async Task<IActionResult> IsUserWatchingRepository(string owner, string repositoryName)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var response = await _remoteRepositoryService.IsUserWatchingRepo(userName, owner, repositoryName);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("star/{owner}/{repositoryName}")]
    public async Task<IActionResult> StarRepository(string owner, string repositoryName)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService.StarRepo(userName, owner, repositoryName);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("star/{owner}/{repositoryName}")]
    public async Task<IActionResult> UnstarRepository(string owner, string repositoryName)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService.UnstarRepo(userName, owner, repositoryName);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("star/{owner}/{repositoryName}")]
    public async Task<IActionResult> IsUserStarringRepository(string owner, string repositoryName)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var response = await _remoteRepositoryService.IsUserStarredRepo(userName, owner, repositoryName);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}