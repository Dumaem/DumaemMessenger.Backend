using Messenger.Domain.Models;

namespace Messenger.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task CreateTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetTokenAsync(string token);
    
    /// <summary>
    /// Sets the IsUsed property to true 
    /// </summary>
    Task UseTokenAsync(int tokenId);

    Task RevokeTokenAsync(int tokenId);
    Task<RefreshToken?> GetTokenByUserAndDeviceIdAsync(int userId, string deviceId);
}