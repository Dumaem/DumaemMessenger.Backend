namespace Messenger.WebAPI.Credentials;

/// <summary>
/// Credentials used by user to register
/// </summary>
public class RegistrationCredentials
{
    public string Email { get; set; }
    public string Password { get; set; }
}