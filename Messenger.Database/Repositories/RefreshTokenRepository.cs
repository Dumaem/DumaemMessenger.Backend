using Messenger.Database.Models;
using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

namespace Messenger.Database.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MessengerContext _context;

    public RefreshTokenRepository(MessengerContext context)
    {
        _context = context;
    }

    public async Task CreateTokenAsync(RefreshToken refreshToken)
    {
        var refreshTokenDb = new RefreshTokenDb
        {
            JwtId = refreshToken.JwtId, CreationDate = refreshToken.CreationDate, UserId = refreshToken.UserId,
            ExpiryDate = refreshToken.ExpiryDate
        };

        _context.RefreshTokens.Add(refreshTokenDb);
        await _context.SaveChangesAsync();
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