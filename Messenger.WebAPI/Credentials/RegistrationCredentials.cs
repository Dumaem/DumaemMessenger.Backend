namespace Messenger.WebAPI.Credentials;

/// <summary>
/// Credentials used by user to register
/// </summary>
public class RegistrationCredentials
{
    public string Name { get; set; } = null!;
    public string? Username { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}