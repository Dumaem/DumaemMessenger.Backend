using Messenger.Domain;
using Messenger.WebAPI.Credentials;
using Messenger.WebAPI.Responses;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = Messenger.Domain.Services.IAuthorizationService;

namespace Messenger.WebAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<AuthorizationController> _logger;

    public AuthorizationController(ILogger<AuthorizationController> logger, IAuthorizationService authorizationService)
    {
        _logger = logger;
        _authorizationService = authorizationService;
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationCredentials credentials)
    {
        var result = await _authorizationService.RegisterAsync(credentials.Email, credentials.Password);

        return VerifyAuthenticationResult(result);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationCredentials credentials)
    {
        var result = await _authorizationService.AuthorizeAsync(credentials.Email, credentials.Password);

        return VerifyAuthenticationResult(result);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshCredentials credentials)
    {
        if (credentials.Token.AccessToken is null || credentials.Token.RefreshToken is null)
        {
            return BadRequest("Tokens cannot be null");
        }
        
        var result = await _authorizationService.RefreshAsync(credentials.Token.AccessToken, credentials.Token.RefreshToken);

        return VerifyAuthenticationResult(result);
    }

    /// <summary>
    /// Returns HTTP responses that is appropriate to the authentication result
    /// </summary>
    private IActionResult VerifyAuthenticationResult(AuthenticationResult result)
    {
        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(new AuthResponse
        {
            AccessToken = result.Token!.AccessToken,
            RefreshToken = result.Token.RefreshToken
        });
    }
}