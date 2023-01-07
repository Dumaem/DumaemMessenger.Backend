using Messenger.WebAPI.Domain.Models;

namespace Messenger.WebAPI.Database.Repositories.Impl;

public class UserRepository : IUserRepository
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return null;
    }
}