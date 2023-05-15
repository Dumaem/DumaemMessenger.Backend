using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User?> GetUserByIdAsync(int id);
    public Task<int> CreateUserAsync(User user, string password);
    public Task<string> GetUserEncryptedPassword(int userId);
    Task<IEnumerable<User>> GetUsers();
    Task<IEnumerable<User>> GetUsers(int count, int offset);
    Task<User?> ChangeName(int id, string name);
    Task<User?> ChangeUsername(int id, string username);
    Task<User?> ChangeEmail(int id, string email);
}