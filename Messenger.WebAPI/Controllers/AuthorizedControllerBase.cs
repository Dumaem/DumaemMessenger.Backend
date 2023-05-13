using System.Security.Claims;
using Messenger.WebAPI.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class AuthorizedControllerBase : ControllerBase
{
    protected UserHttpClaims ParseHttpClaims()
    {
        var email = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
        var id = int.Parse(HttpContext.User.Claims.First(x => x.Type == "id").Value);
        return new UserHttpClaims { Email = email, Id = id };
    }
}