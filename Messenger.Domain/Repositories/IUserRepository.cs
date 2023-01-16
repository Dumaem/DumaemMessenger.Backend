using Messenger.Domain.Models;

namespace Messenger.Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User?> GetUserByIdAsync(int id);
    public Task<int> CreateUserAsync(User user, string password);
    public Task<string> GetUserEncryptedPassword(int userId);
}