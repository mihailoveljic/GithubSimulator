using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Core.Models.ValueObjects;
using GitHubSimulator.Dtos.Users;
using GitHubSimulator.Factories;
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
            var result = await _authenticationService.Authenticate(Mail.Create(dto.Email), dto.Password);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
    {
        try
        {
            await _userService.Insert(_userFactory.MapToDomain(dto));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
    {
        try
        {
            await _userService.Update(_userFactory.MapToDomain(dto));
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeleteUser([FromQuery] Guid id)
    {
        return Ok(await _userService.Delete(id));
    }
}
