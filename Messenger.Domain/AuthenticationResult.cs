using Messenger.Domain.Models;

namespace Messenger.Domain;

public class AuthenticationResult
{
    public JwtToken? Token { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
}