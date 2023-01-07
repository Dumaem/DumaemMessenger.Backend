namespace Messenger.WebAPI.Authentication;

/// <summary>
/// Structure that contains tokens that are returned to the client
/// </summary>
public class JwtToken
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}