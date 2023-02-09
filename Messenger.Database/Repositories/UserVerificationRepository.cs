using Dapper;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Repositories;

namespace Messenger.Database.Repositories;

public class UserVerificationRepository : IUserVerificationRepository
{
    private readonly MessengerReadonlyContext _readonlyContext;

    public UserVerificationRepository(MessengerContext context, MessengerReadonlyContext readonlyContext)
    {
        _readonlyContext = readonlyContext;
    }

    public async Task<(string?, DateTime)> GetExistingVerifyToken(int userId)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<(string?, DateTime)>(
            UserVerificationRepositoryQueries.GetVerifyToken, new {userId});
        return res;
    }

    public async Task CreateVerifyToken(string token, DateTime expiryDate, int userId)
    {
        await _readonlyContext.Connection.ExecuteAsync(UserVerificationRepositoryQueries.CreateToken, new
        {
            token,
            expiryDate,
            userId
        });
    }

    public async Task VerifyUser(int userId)
    {
        await _readonlyContext.Connection.ExecuteAsync(UserVerificationRepositoryQueries.VerifyUser,
            new {id = userId});
    }

    public async Task RevokeExpiredToken(string token)
    {
        await _readonlyContext.Connection.ExecuteAsync(UserVerificationRepositoryQueries.RevokeExpiredToken,
            new {token});
    }
}