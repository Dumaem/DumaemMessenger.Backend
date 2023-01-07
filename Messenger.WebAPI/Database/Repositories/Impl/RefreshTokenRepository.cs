using Messenger.WebAPI.Domain.Models;

namespace Messenger.WebAPI.Database.Repositories.Impl;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    public Task CreateTokenAsync(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task UseTokenAsync()
    {
        throw new NotImplementedException();
    }
}