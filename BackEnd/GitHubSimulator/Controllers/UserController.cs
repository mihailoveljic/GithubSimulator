using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Dtos.Users;
using GitHubSimulator.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GitHubSimulator.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserFactory _userFactory;

    public UserController(IUserService userService, UserFactory userFactory)
    {
        _userService = userService;
        _userFactory = userFactory;
    }

    [HttpPost]
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
