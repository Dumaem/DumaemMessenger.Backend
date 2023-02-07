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

    public async Task<bool> GetExistingVerifyToken(int userId)
    {
        return await _readonlyContext.Connection.QuerySingleOrDefaultAsync<bool>(
            UserVerificationRepositoryQueries.GetVerifyToken, new {userId});
    }

    public async Task CreateVerifyToken(string token, DateTime expiryDate, int userId)
    {
        await _readonlyContext.Connection.ExecuteAsync(UserVerificationRepositoryQueries.CreateToken ,new
        {
            token,
            expiryDate,
            userId
        });
    }
}