using Microsoft.AspNetCore.Http;

namespace Messenger.Domain.Services;

public interface IAuthorizationService
{
    public Task<AuthenticationResult> RegisterAsync(string name, string email, string password, string? username,
        string userAgent);
    public Task<AuthenticationResult> AuthorizeAsync(string email, string password, string userAgent);
    public Task<AuthenticationResult> RefreshAsync(string token, string refreshToken, string userAgent);
}