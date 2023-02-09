namespace Messenger.Domain.Repositories;

public interface IUserVerificationRepository
{
    public Task<(string?, DateTime)> GetExistingVerifyToken(int userId);
    public Task CreateVerifyToken(string token, DateTime expiryDate, int userId);
    public Task VerifyUser(int userId);
    public Task RevokeExpiredToken(string token);
}