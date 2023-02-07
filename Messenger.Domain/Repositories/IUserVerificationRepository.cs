namespace Messenger.Domain.Repositories;

public interface IUserVerificationRepository
{
    public Task<bool> GetExistingVerifyToken(int userId);
    public Task CreateVerifyToken(string token, DateTime expiryDate, int userId);
}