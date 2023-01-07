using Messenger.WebAPI.Database.Repositories;
using Messenger.WebAPI.Domain.Models;

namespace Messenger.WebAPI.Domain.Services.Impl;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<bool> CreateUser(User user, string password)
    {
        // TODO: избавиться от затычки
        user.Id = 1;
        return Task.FromResult(true);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        // TODO: избавиться от затычки
        return Task.FromResult(new User {Id = 1, Email = "123123", Username = "123123"});
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckUserPassword(User user, string password)
    {
        // TODO: избавиться от затычки
        return Task.FromResult(true);
    }
}