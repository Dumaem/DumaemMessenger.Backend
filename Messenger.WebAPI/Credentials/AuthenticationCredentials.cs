namespace Messenger.WebAPI.Credentials;

/// <summary>
/// Credentials used by user to log in
/// </summary>
public class AuthenticationCredentials
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}