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
    
    /// <summary>
    /// Finds actual(not used and not revoked) token by userId and deviceId
    /// Sets the IsRevoked property to true
    /// </summary>
    Task RevokeTokenIfExistsAsync(int userId, string deviceId);
    Task<RefreshToken?> GetTokenByUserAndDeviceIdAsync(int userId, string deviceId);
}