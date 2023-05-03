using Messenger.Domain.Models;

namespace Messenger.Domain.Repositories;

public interface IUserVerificationRepository
{
    public Task<(string?, DateTime)> GetExistingVerifyToken(int userId);
    public Task<User?> GetUserByToken(string token);
    public Task CreateVerifyToken(string token, DateTime expiryDate, int userId);
    public Task VerifyUser(int userId);
    public Task RevokeExpiredToken(string token);
}