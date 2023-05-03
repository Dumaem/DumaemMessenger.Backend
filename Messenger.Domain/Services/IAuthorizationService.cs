using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IAuthorizationService
{
    public Task<AuthenticationResult> RegisterAsync(string name, string email, string password, string? username,
        string userAgent);

    public Task<AuthenticationResult> AuthorizeAsync(string email, string password, string userAgent);
    public Task<AuthenticationResult> RefreshAsync(string token, string refreshToken, string userAgent);
    public Task<VerificationResult> GenerateUserVerifyToken(string userEmail);
    public Task<BaseResult> VerifyUser(string token);
}