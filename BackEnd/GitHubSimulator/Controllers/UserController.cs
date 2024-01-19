using System.Security.Claims;
using CSharpFunctionalExtensions;
using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Dtos.Users;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;
    private readonly UserFactory _userFactory;

    public UserController(IUserService userService, UserFactory userFactory, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _userFactory = userFactory;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        try
        {
            var result = await _authenticationService.Authenticate(dto.Email, dto.Password);
            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
    {
        try
        {
            await _userService.Insert(_userFactory.MapToDomain(dto, isAdmin: false));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost("registerAdmin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] UserRegistrationDto dto)
    {
        try
        {
            await _userService.Insert(_userFactory.MapToDomain(dto, isAdmin: true));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<GetUserDto>> UpdateUser([FromBody] UpdateUserDto dto)
    {
        try
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = await _userService.Update(_userFactory.MapToDomain(dto, userId, isAdmin: false));
            return Ok(_userFactory.MapToDto(user.Value));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteUser([FromQuery] Guid id)
    {
        return Ok(await _userService.Delete(id));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetUserDto>> GetUser()
    {
        try
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            return Ok(_userFactory.MapToDto(await _userService.GetById(userId)));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
