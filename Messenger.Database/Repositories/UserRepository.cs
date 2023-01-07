using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

namespace Messenger.Database.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User?> GetUserByEmail(string email)
    {
        // TODO: избавиться от затычки
        return Task.FromResult(new User());
    }
}