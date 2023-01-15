using Messenger.Domain.Models;

namespace Messenger.Domain.Services;

public interface IUserService
{
    Task<int> CreateUserAsync(User user, string password);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task<bool> CheckUserPasswordAsync(User user, string password);
}