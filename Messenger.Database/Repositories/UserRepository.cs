using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

namespace Messenger.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MessengerContext _context;

    public UserRepository(MessengerContext context)
    {
        _context = context;
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        // TODO: избавиться от затычки
        return Task.FromResult(new User());
    }

    public async Task<int?> CreateUserAsync(User user, string password)
    {
        var dbUser = new Models.UserDb
        {
            Name = user.Username
        };

        _context.Users.Add(dbUser);
        await _context.SaveChangesAsync();
        return dbUser.Id;
    }
}