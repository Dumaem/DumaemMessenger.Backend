using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IUserService
{
    Task<int> CreateUserAsync(User user, string password);
    Task<User?> GetUserAsync(string email);
    Task<User?> GetUserAsync(int id);
    Task<IEnumerable<User>?> GetUsersAsync();
    Task<IEnumerable<User>?> GetUsersAsync(int count, int offset);
    Task<bool> CheckUserPasswordAsync(int userId, string password);
    Task<EntityResult<User>> ChangeName(int id, string name);
    Task<EntityResult<User>> ChangeUsername(int id, string username);
    Task<EntityResult<User>> ChangeEmail(int id, string email);
}