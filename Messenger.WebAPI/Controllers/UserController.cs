using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Services;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

public class UserController : AuthorizedControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [Route("user-by-id")]
    public async Task<IActionResult> GetUser([FromQuery] int id)
    {
        var result = await _userService.GetUserAsync(id);
        if (result is null)
            return BadRequest(UserErrorMessage.NotExistUser);
        return Ok(result);
    }

    [HttpGet]
    [Route("user-by-email")]
    public async Task<IActionResult> GetUser([FromQuery] string email)
    {
        var result = await _userService.GetUserAsync(email);
        if (result is null)
            return BadRequest(UserErrorMessage.NotExistUser);
        return Ok(result);
    }

    [HttpGet]
    [Route("users")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetUsersAsync();
        if (result is null)
            return BadRequest();
        return Ok(result);
    }

    [HttpGet]
    [Route("users-list")]
    public async Task<IActionResult> GetUsers([FromQuery] int count, [FromQuery] int offset)
    {
        var result = await _userService.GetUsersAsync(count, offset);
        if (result is null)
            return BadRequest();
        return Ok(result);
    }

    [HttpPut]
    [Route("changeName")]
    public async Task<IActionResult> ChangeName([FromQuery] int id, [FromQuery] string name)
    {
        var result = await _userService.ChangeName(id, name);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Entity);
    }
    
    [HttpPut]
    [Route("changeUsername")]
    public async Task<IActionResult> ChangeUsername([FromQuery] int id, [FromQuery] string username)
    {
        var result = await _userService.ChangeUsername(id, username);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Entity);
    }
    
    [HttpPut]
    [Route("changeEmail")]
    public async Task<IActionResult> ChangeEmail([FromQuery] int id, [FromQuery] string email)
    {
        var result = await _userService.ChangeEmail(id, email);
        if (!result.Success)
            return BadRequest(result.Message);
        return Ok(result.Entity);
    }
}