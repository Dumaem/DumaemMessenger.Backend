namespace Messenger.WebAPI.Responses;

/// <summary>
/// Authentication result from controller
/// </summary>
public class AuthResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}