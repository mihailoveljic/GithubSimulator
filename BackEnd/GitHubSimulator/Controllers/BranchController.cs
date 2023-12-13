using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.BuildingBlocks.Exceptions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Branches;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BranchController : ControllerBase
{
	public BranchController(IBranchService branchService, BranchFactory branchFactory)
	{
		_branchService = branchService;
		_branchFactory = branchFactory;
	}

	private readonly IBranchService _branchService;
	private readonly BranchFactory _branchFactory;

	[HttpGet(Name = "GetAllBranches")]
	public async Task<IActionResult> GetAll()
	{
		try
		{
			return Ok(await _branchService.GetAll());
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}

	[HttpGet("{id:guid}", Name = "GetBranchById")]
	[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
	public async Task<IActionResult> GetBranchById(Guid id)
	{
		var response = await _branchService.GetById(id);
		if (response is null)
		{
			return NotFound("A branch with the provided ID not found");
		}

		return Ok(response);
	}

	[HttpPost]
	public async Task<IActionResult> CreateBranch([FromBody] InsertBranchDto dto)
	{
		try
		{
			var result = await _branchService.Insert(_branchFactory.MapToDomain(dto));
			return Created("Branch successfully crated", result);
		}
		catch (InvalidRepositoryForBranchException er)
		{
			return BadRequest(er.Message);
		}
		catch (InvalidIssueForBranchException es)
		{
			return BadRequest(es.Message);
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

	// TODO Stefan: not sure how to handle commits
	[HttpPut]
	[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
	public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchDto dto)
	{
		try
		{
			var response = await _branchService.Update(_branchFactory.MapToDomain(dto));
			if (response.Equals(Maybe<Branch>.None))
			{
				return NotFound("A branch with the provided ID not found");
			}

			return Ok(response.Value);
		}
		catch (InvalidIssueForBranchException es)
		{
			return BadRequest(es.Message);
		}
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteBranch(Guid id)
	{
		var response = await _branchService.Delete(id);
		if (response)
		{
			return Ok("Branch deleted successfully");
		}

		return NotFound("Branch with the provided ID not found");
	}
}