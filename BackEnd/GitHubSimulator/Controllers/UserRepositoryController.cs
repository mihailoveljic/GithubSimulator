using System.Security.Claims;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Interfaces.Services;
using GitHubSimulator.Core.Models.Entities;
using GitHubSimulator.Core.Models.Enums;
using GitHubSimulator.Dtos.UserRepositories;
using GitHubSimulator.Factories;
using GitHubSimulator.Infrastructure.RemoteRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class UserRepositoryController : ControllerBase
{
    private readonly IUserRepositoryService _userRepositoryService;
    private readonly UserRepositoryFactory _userRepositoryFactory;
    private readonly IUserService _userService;
    private readonly IRemoteRepositoryService _remoteRepositoryService;
    private readonly IRepositoryService _repositoryService;

    public UserRepositoryController(IUserRepositoryService userRepositoryService, IUserService userService, UserRepositoryFactory userRepositoryFactory, IRemoteRepositoryService remoteRepositoryService, IRepositoryService repositoryService)
    {
        _userRepositoryService = userRepositoryService;
        _userService = userService;
        _userRepositoryFactory = userRepositoryFactory;
        _remoteRepositoryService = remoteRepositoryService;
        _repositoryService = repositoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _userRepositoryService.GetAll();
            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("GetByUserName", Name = "GetByUserName")]
    public async Task<IActionResult> GetByUserName([FromBody] GetUserRepositoriesByUserNameDto dto)
    {
        try
        {
            var user = await _userService.GetByEmail(dto.User.Email);
            
            var result = await _userRepositoryService.GetByUserName(user.AccountCredentials.UserName);
            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("GetAuthenticatedUserUserRepos", Name = "GetAuthenticatedUserUserRepositories")]
    public async Task<IActionResult> GetAuthenticatedUserUserRepositories()
    {
        try
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value!;
            
            var result = await _userRepositoryService.GetByUserName(userName);
            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("GetByRepoName", Name = "GetByRepositoryName")]
    public async Task<IActionResult> GetByRepositoryName([FromBody] GetUserRepositoriesByRepositoryNameDto dto)
    {
        try
        {
            var result = await _userRepositoryService.GetByRepositoryName(dto.RepositoryName);
            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("GetByRepoNameAlt", Name = "GetByRepositoryNameAlternative")]
    public async Task<IActionResult> GetByRepositoryNameAlternative([FromBody] GetUserRepositoriesByRepositoryNameDto dto)
    {
        try
        {
            var userRepos = await _userRepositoryService.GetByRepositoryName(dto.RepositoryName);
            var userRepositories = userRepos.ToList();
            if (!userRepositories.Any()) return NotFound();
            
            var result = new List<GetUserRepositoriesByRepositoryNameAltResultDto>();
            foreach (var ur in userRepositories)
            {
                var user = await _userService.GetByUsername(ur.UserName);
                var userEmail = user.Mail.Email;
                    
                result.Add(_userRepositoryFactory.MapToDto(ur, userEmail));
            }
            return Ok(result);

        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("AddUserToRepo", Name = "AddUserToRepository")]
    public async Task<IActionResult> AddUserToRepository([FromBody] AddUserToRepositoryDto dto)
    {
        try
        {
            var user = await _userService.GetByEmail(dto.User.Email);
            
            var result = await _userRepositoryService
                .AddUserToRepository(_userRepositoryFactory
                    .MapToDomain(user.AccountCredentials.UserName, dto.RepositoryName, UserRepositoryRole.Write));

            var owner = await _repositoryService.GetRepositoryOwner(dto.RepositoryName);
            await _remoteRepositoryService.AddCollaboratorToRepository
                (owner, dto.RepositoryName, user.AccountCredentials.UserName);
            
            return Created("User successfully added to the repository", 
                _userRepositoryFactory.MapToDto(result, user.Mail.Email));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPost("RemoveUserFromRepo", Name = "RemoveUserFromRepository")]
    public async Task<IActionResult> RemoveUserFromRepository([FromBody] AddUserToRepositoryDto dto)
    {
        try
        {
            var user = await _userService.GetByEmail(dto.User.Email);
            var response =
                await _userRepositoryService.RemoveUserFromRepository(user.AccountCredentials.UserName,
                    dto.RepositoryName);
            
            if (!response) return NotFound("UserRepository with provided information not found");
            
            var owner = await _repositoryService.GetRepositoryOwner(dto.RepositoryName);
            await _remoteRepositoryService.RemoveCollaboratorFromRepository(
                owner, dto.RepositoryName, user.AccountCredentials.UserName);
                
            return NoContent();

        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("ChangeUserRole", Name = "ChangeUserRole")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleDto dto)
    {
        try
        {
            var user = await _userService.GetByEmail(dto.User.Email);

            var response = await _userRepositoryService.ChangeUserRole(user.AccountCredentials.UserName,
                dto.RepositoryName, dto.NewRole);
            if (response.Equals(Maybe<UserRepository>.None))
            {
                return NotFound("UserRepository with provided information not found");
            }

            return Ok(response.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}