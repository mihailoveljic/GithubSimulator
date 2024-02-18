﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.AggregateRoots;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;
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
    private readonly IUserService _userService;
    private readonly IUserRepositoryService _userRepositoryService;
    private readonly UserRepositoryFactory _userRepositoryFactory;

    public RepositoryController(IRepositoryService repositoryService, IRemoteRepositoryService remoteRepositoryService, RepositoryFactory repositoryFactory, ICacheService cacheService, IUserService userService, IUserRepositoryService userRepositoryService, UserRepositoryFactory userRepositoryFactory)
    {
        _repositoryService = repositoryService;
        _remoteRepositoryService = remoteRepositoryService;
        _repositoryFactory = repositoryFactory;
        _cacheService = cacheService;
        _userService = userService;
        _userRepositoryService = userRepositoryService;
        _userRepositoryFactory = userRepositoryFactory;
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
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        var cachedRepositories = await _cacheService.GetAllRepositoriesAsync(userName);

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
            
            var result = await _repositoryService.Insert(_repositoryFactory.MapToDomain(dto, userName));
            await _cacheService.RemoveAllRepositoryDataAsync(); // Invalidate cache

            await _userRepositoryService.AddUserToRepository(_userRepositoryFactory
                .MapToDomain(userName, result.Name, UserRepositoryRole.Owner));
            
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

    [HttpGet("GetByName/{repo}", Name = "GetRepositoryByName")]
    public async Task<IActionResult> GetRepositoryByName(string repo)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var result = await _remoteRepositoryService.GetRepositoryByName(userName, repo);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }

    [HttpGet("GetRepoOwner/{repo}", Name = "GetRepositoryOwner")]
    public async Task<IActionResult> GetRepositoryOwner(string repo)
    {
        try
        {
            var result = await _repositoryService.GetRepositoryOwner(repo);
            return Ok(result);
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

    [HttpPut("UpdateName", Name = "UpdateRepositoryName")]
    public async Task<IActionResult> UpdateRepositoryName([FromBody] UpdateRepositoryNameDto dto)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService
                    .UpdateRepositoryName(userName, dto.RepositoryName, dto.NewName);

            var response = await _repositoryService.UpdateName(dto.RepositoryName, dto.NewName);
            
            if (response.Equals(Maybe<Repository>.None))
            {
                return NotFound("A repository with the provided ID not found");
            }

            return Ok(response.Value);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }
    
    [HttpPut("UpdateVisibility", Name = "UpdateRepositoryVisibility")]
    public async Task<IActionResult> UpdateRepositoryName([FromBody] UpdateRepositoryVisibilityDto dto)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            await _remoteRepositoryService
                .UpdateRepositoryVisibility(userName, dto.RepositoryName, dto.IsPrivate);

            var response = await _repositoryService.UpdateVisibility(dto.RepositoryName, dto.IsPrivate);
            
            if (response.Equals(Maybe<Repository>.None))
            {
                return NotFound("A repository with the provided ID not found");
            }

            return Ok(response.Value);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }
    
    [HttpPut("UpdateArchivedState", Name = "UpdateRepositoryArchivedState")]
    public async Task<IActionResult> UpdateRepositoryArchivedState([FromBody] UpdateRepositoryArchivedState dto)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var result = await _remoteRepositoryService
                .UpdateRepositoryArchivedState(userName, dto.RepositoryName, dto.IsArchived);
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }

    [HttpPost("UpdateOwner", Name = "UpdateRepositoryOwner")]
    public async Task<IActionResult> UpdateRepositoryOwner([FromBody] UpdateRepositoryOwner dto)
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            var newOwner = await _userService.GetByEmail(dto.NewOwner.Email);
            
            await _remoteRepositoryService
                .UpdateRepositoryOwner(userName, dto.RepositoryName, newOwner.AccountCredentials.UserName);

            var userRepoResponse = await _userRepositoryService.ChangeUserRole(
                newOwner.AccountCredentials.UserName, dto.RepositoryName, UserRepositoryRole.Owner);
            
            if (userRepoResponse.Equals(Maybe<UserRepository>.None))
            {
                return NotFound("Cannot change user repository role, user repository with the provided information not found");
            }

            var removeResponse = await _userRepositoryService.RemoveUserFromRepository(userName, dto.RepositoryName);
            if (!removeResponse)
            {
                return NotFound("Cannot remove user from repository, user repository with the provided information not found");
            }
            
            var response = await _repositoryService.UpdateRepositoryOwner(dto.RepositoryName, newOwner.AccountCredentials.UserName);
            if (response.Equals(Maybe<Repository>.None))
            {
                return NotFound("A repository with the provided ID not found");
            }
            
            return Ok(response.Value);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal Server Error: " + e.Message);
        }
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteRepository([FromRoute] string name)
    {
        var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
        
        await _remoteRepositoryService.DeleteRepository(userName, name);
        
        var response = await _repositoryService.Delete(name);
        if (!response) return NotFound("Repository with provided id not found");

        var userRepositories = await _userRepositoryService.GetByRepositoryName(name);
        IEnumerable<UserRepository> userRepos = userRepositories as List<UserRepository> ?? userRepositories.ToList();
        if (userRepos.Any())
        {
            foreach (var userRepo in userRepos)
            {
                await _userRepositoryService.RemoveUserFromRepository(userRepo.UserName, userRepo.RepositoryName);
            }
        }
        
        await _cacheService.RemoveAllRepositoryDataAsync();
        return NoContent();
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

}