using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Services;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    [Route("get-user-by-id")]
    public async Task<IActionResult> GetUserById([FromQuery] int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result is null)
            return BadRequest(UserErrorMessage.NotExistUser);
        return Ok(result);
    }
    [HttpGet]
    [Route("get-user-by-email")]
    public async Task<IActionResult> GetUserById([FromQuery] string email)
    {
        var result = await _userService.GetUserByEmailAsync(email);
        if (result is null)
            return BadRequest(UserErrorMessage.NotExistUser);
        return Ok(result);
    }
}