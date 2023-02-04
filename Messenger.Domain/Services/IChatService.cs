using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IChatService
{
    public Task<BaseResult> CreateChatAsync(IEnumerable<User> participants);
    public Task<IEnumerable<Chat>> GetChatsForUserAsync(string email);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName);
}