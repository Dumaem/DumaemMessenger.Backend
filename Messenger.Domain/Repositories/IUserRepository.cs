using Messenger.Domain.Models;

namespace Messenger.Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<int?> CreateUserAsync(User user, string password);
}