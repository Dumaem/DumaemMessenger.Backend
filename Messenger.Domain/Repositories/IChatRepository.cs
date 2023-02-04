using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Repositories;

public interface IChatRepository
{
    public Task<DatabaseCreateResult> CreateChatAsync(IEnumerable<User> participants);
    public Task<IEnumerable<Chat>> GetChatsForUserAsync(string email);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName);
}