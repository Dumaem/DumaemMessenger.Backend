using Messenger.WebAPI.Domain.Models;

namespace Messenger.WebAPI.Database.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmail(string email);
}