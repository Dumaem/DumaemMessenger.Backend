using Dapper;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;
using Microsoft.EntityFrameworkCore;

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
                Username = res.Username, Name = res.Name, Email = res.Email, Id = res.Id,
                IsVerified = res.IsVerified
            };
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<UserDb>(
            UserRepositoryQueries.GetUserById, new {id});

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

    public Task<IEnumerable<User>> GetUsers()
    {
        return Task.FromResult(_context.Users.Select(EntityConverter.ConvertUser));
    }

    public Task<IEnumerable<User>> GetUsers(int count, int offset)
    {
        return  Task.FromResult(_context.Users.Skip(offset).Take(count)
            .AsEnumerable()
            .Select(EntityConverter.ConvertUser));
    }

    public async Task<User?> ChangeName(int id, string name)
    {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (dbUser is null)
            return null;
        dbUser.Name = name;
        await _context.SaveChangesAsync();
        return EntityConverter.ConvertUser(dbUser);
    }
    
    public async Task<User?> ChangeUsername(int id, string username)
    {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (dbUser is null)
            return null;
        dbUser.Username = username;
        await _context.SaveChangesAsync();
        return EntityConverter.ConvertUser(dbUser);
    }

    public async Task<User?> ChangeEmail(int id, string email)
    {
        var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (dbUser is null)
            return null;
        dbUser.Email = email;
        await _context.SaveChangesAsync();
        return EntityConverter.ConvertUser(dbUser);
    }
}