using Messenger.WebAPI.Authentication;

namespace Messenger.WebAPI.Credentials;

/// <summary>
/// Credentials used by user to refresh the access token
/// </summary>
public class RefreshCredentials
{
    public JwtToken Token { get; set; }
}