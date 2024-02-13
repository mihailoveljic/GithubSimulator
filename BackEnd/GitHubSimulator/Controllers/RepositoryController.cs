using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Dtos.Repositories;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class RepositoryController : ControllerBase
{
	private readonly IRepositoryService _repositoryService;
	private readonly RepositoryFactory _repositoryFactory;
	private readonly ICacheService _cacheService;

	public RepositoryController(
		IRepositoryService repositoryService, 
		RepositoryFactory repositoryFactory,
		ICacheService cacheService)
	{
		_repositoryService = repositoryService;
		_repositoryFactory = repositoryFactory;
		_cacheService = cacheService;
	}

    [HttpGet(Name = "GetAllRepositories")]
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
				foreach(var repo in repositories)
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

    [HttpPost]
    public async Task<IActionResult> CreateRepository([FromBody] InsertRepositoryDto dto)
    {
        try
        {
            var result = await _repositoryService.Insert(_repositoryFactory.MapToDomain(dto));
            // Invalidate or update cache as necessary
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

}