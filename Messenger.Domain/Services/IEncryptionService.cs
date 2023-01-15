namespace Messenger.Domain.Services;

public interface IEncryptionService
{
    public Task<string> EncryptString(string content);
}