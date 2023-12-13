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

	public RepositoryController(IRepositoryService repositoryService, RepositoryFactory repositoryFactory)
	{
		_repositoryService = repositoryService;
		_repositoryFactory = repositoryFactory;
	}

	[HttpGet(Name = "GetAllRepositories")]
	public async Task<IActionResult> GetAllRepositories()
	{
		try
		{
			return Ok(await _repositoryService.GetAll());
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
	public async Task<IActionResult> DeleteRepository([FromQuery] Guid id)
	{
		var response = await _repositoryService.Delete(id);
		if (response)
		{
			return NoContent();
		}

		return NotFound("Repository with provided id not found");
	}
}