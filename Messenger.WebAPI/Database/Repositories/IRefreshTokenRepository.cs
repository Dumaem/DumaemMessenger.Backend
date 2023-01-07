using Messenger.WebAPI.Domain.Models;

namespace Messenger.WebAPI.Database.Repositories;

public interface IRefreshTokenRepository
{
    Task CreateTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetTokenAsync(string token);
    
    /// <summary>
    /// Sets the IsUsed property to true 
    /// </summary>
    Task UseTokenAsync();
}