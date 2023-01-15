namespace Messenger.Domain.Services;

public interface IAuthorizationService
{
    public Task<AuthenticationResult> RegisterAsync(string email, string password);
    public Task<AuthenticationResult> AuthorizeAsync(string email, string password);
    public Task<AuthenticationResult> RefreshAsync(string token, string refreshToken);
}