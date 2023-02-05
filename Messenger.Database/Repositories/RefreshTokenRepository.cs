using Dapper;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

namespace Messenger.Database.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MessengerContext _context;
    private readonly MessengerReadonlyContext _readonlyContext;

    public RefreshTokenRepository(MessengerContext context, MessengerReadonlyContext readonlyContext)
    {
        _context = context;
        _readonlyContext = readonlyContext;
    }

    public async Task CreateTokenAsync(RefreshToken refreshToken)
    {
        var refreshTokenDb = new RefreshTokenDb
        {
            JwtId = refreshToken.JwtId, CreationDate = refreshToken.CreationDate, UserId = refreshToken.UserId,
            ExpiryDate = refreshToken.ExpiryDate,
            DeviceId = refreshToken.DeviceId,
            Token = refreshToken.Token
        };

        _context.RefreshTokens.Add(refreshTokenDb);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetTokenAsync(string token)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<RefreshTokenDb>(
            RefreshTokenRepositoryQueries.GetRefreshToken, new {token});
        
        return res is null
            ? null
            : new RefreshToken
            {
                Id = res.Id,
                JwtId = res.JwtId,
                IsUsed = res.IsUsed,
                IsRevoked = res.IsRevoked,
                CreationDate = res.CreationDate,
                ExpiryDate = res.ExpiryDate,
                UserId = res.UserId,
                DeviceId = res.DeviceId
            };
    }

    public async Task UseTokenAsync(int tokenId)
    {
        await  _readonlyContext.Connection
            .ExecuteAsync(RefreshTokenRepositoryQueries.UpdateRefreshTokenUse, new {id = tokenId});
    }
    
    public async Task RevokeTokenAsync(int tokenId)
    {
        await  _readonlyContext.Connection
            .ExecuteAsync(RefreshTokenRepositoryQueries.UpdateRefreshTokenRevoke, new {id = tokenId});
    }

    public async Task<RefreshToken?> GetTokenByUserAndDeviceIdAsync(int userId, string deviceId)
    {
       var res =  await _readonlyContext.Connection
            .QuerySingleOrDefaultAsync<RefreshTokenDb>(RefreshTokenRepositoryQueries.GetTokenByUserAndDeviceId,
                new {userId, deviceId});
       
       return res is null
           ? null
           : new RefreshToken
           {
               Id = res.Id,
               JwtId = res.JwtId,
               IsUsed = res.IsUsed,
               IsRevoked = res.IsRevoked,
               CreationDate = res.CreationDate,
               ExpiryDate = res.ExpiryDate,
               UserId = res.UserId,
               DeviceId = res.DeviceId
           };
    }
}