using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.BuildingBlocks.Exceptions;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Dtos.Branches;
using GitHubSimulator.Infrastructure.RemoteBranch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BranchFactory = GitHubSimulator.Factories.BranchFactory;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BranchController : ControllerBase
{
	public BranchController(IBranchService branchService, BranchFactory branchFactory, ICacheService cacheService, IRemoteBranchService remoteBranchService)
	{
        _remoteBranchService = remoteBranchService;
        _branchService = branchService;
		_branchFactory = branchFactory;
		_cacheService = cacheService;
	}

    private readonly IRemoteBranchService _remoteBranchService;
    private readonly IBranchService _branchService;
	private readonly BranchFactory _branchFactory;
	private readonly ICacheService _cacheService;

    [HttpGet(Name = "GetAllBranches")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var cachedBranches = await _cacheService.GetAllBranchesAsync();

            if (cachedBranches != null && cachedBranches.Any())
            {
                Console.WriteLine("IZ KESAAAAAAAAAAAAAAAAAAAAAAAAA");
                return Ok(cachedBranches);
            }

            var branches = await _branchService.GetAll();
            if (branches.Any())
            {
                foreach (var branch in branches)
                {
                    await _cacheService.SetBranchData(branch, DateTimeOffset.UtcNow.AddHours(2));
                }
            }
            Console.WriteLine("BEZZZZZZZZZZZZZZZ KESAAAAAAAAAAAAAAAAAAAAAAAAA");
            return Ok(branches);
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
    public async Task<IActionResult> CreateBranch([FromQuery] string repo, [FromBody] InsertBranchDto dto)
    {
        try
        {
            // The assumption here is that your factory or service method handles commits internally
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var branch = _branchFactory.MapToDomain(dto); // Map DTO to domain entity, including commits if applicable

            if (dto.Old_Branch_Name != "")
            {
                var oldBranch = await _remoteBranchService.GetBranch(userName, repo, dto.Old_Branch_Name);
                oldBranch.Name = dto.New_Branch_Name;
                var res = await _remoteBranchService.UpdateBranch(userName, repo, oldBranch);

            }
            else
            {
                await _remoteBranchService.CreateBranch(userName, repo, _branchFactory.MapToGiteaDto(dto));
            }

            var result = await _branchService.Insert(branch);

            // Invalidate cache after successful creation
            await _cacheService.RemoveAllBranchDataAsync(); // Adjust method name as needed

         
            return Created("Branch successfully created", result); // Note the typo correction
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
    public async Task<IActionResult> UpdateBranch([FromBody] UpdateBranchDto dto)
    {
        try
        {
            // Similar to creation, assume updates can involve commits logic
            var branchUpdate = _branchFactory.MapToDomain(dto); // Map DTO to domain entity, including updating commits
            var response = await _branchService.Update(branchUpdate);

            if (response.Equals(Maybe<Branch>.None))
            {
                return NotFound("A branch with the provided ID not found");
            }

            // Invalidate cache after successful update
            await _cacheService.RemoveAllBranchDataAsync(); // Ensure cache reflects updated state

            return Ok(response.Value);
        }
        catch (InvalidIssueForBranchException es)
        {
            return BadRequest(es.Message);
        }
        // Handle other exceptions as needed
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBranch(Guid id)
    {
        var response = await _branchService.Delete(id);
        if (response)
        {
            await _cacheService.RemoveAllBranchDataAsync();
            return Ok("Branch deleted successfully");
        }

        return NotFound("Branch with the provided ID not found");
    }


    [HttpDelete("Delete/{repo}/{branch}")]
    public async Task<IActionResult> DeleteBranch(string repo, string branch)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        await _remoteBranchService.DeleteBranch(userName, repo, branch);
       
            return Ok("Branch deleted successfully");
    }

    [HttpGet("RepoBranches")]
    public async Task<IActionResult> GetRepoBranches([FromQuery] string repo,[FromQuery] int page, [FromQuery] int limit)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var response = await _remoteBranchService.GetRepoBranches(userName, repo, page, limit);
            if (response is null)
            {
                return NotFound("User has no branches");
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}