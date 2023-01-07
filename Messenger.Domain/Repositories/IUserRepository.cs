using Messenger.Domain.Models;

namespace Messenger.Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmail(string email);
}