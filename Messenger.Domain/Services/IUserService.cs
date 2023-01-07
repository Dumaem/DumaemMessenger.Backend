using Messenger.Domain.Models;

namespace Messenger.Domain.Services;

public interface IUserService
{
    Task<bool> CreateUser(User user, string password);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task<bool> CheckUserPassword(User user, string password);
}