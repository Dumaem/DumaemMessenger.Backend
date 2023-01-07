using Messenger.WebAPI.Authentication;

namespace Messenger.WebAPI.Domain;

public class AuthenticationResult
{
    public JwtToken? Token { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}