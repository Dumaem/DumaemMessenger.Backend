using Dapper;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

namespace Messenger.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MessengerContext _context;
    private readonly MessengerReadonlyContext _readonlyContext;

    public UserRepository(MessengerContext context, MessengerReadonlyContext readonlyContext)
    {
        _context = context;
        _readonlyContext = readonlyContext;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<UserDb>(
            UserRepositoryQueries.GetUserByEmail, new {email});

        return res is null
            ? null
            : new User
            {
                Username = res.Username, Name = res.Name, Email = res.Email, Id = res.Id
            };
    }

    public async Task<int> CreateUserAsync(User user, string password)
    {
        var dbUser = new UserDb
        {
            Name = user.Name, Username = user.Username, Password = password, Email = user.Email
        };

        _context.Users.Add(dbUser);
        await _context.SaveChangesAsync();
        return dbUser.Id;
    }

    public async Task<string> GetUserEncryptedPassword(int userId)
    {
        return await _readonlyContext.Connection.QuerySingleOrDefaultAsync<string>(
            UserRepositoryQueries.GetUserEncryptedPassword, new {id = userId});
    }
}