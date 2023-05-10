using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IChatService
{
    public Task<BaseResult> CreateChatAsync(IEnumerable<User> participants);
    public Task<BaseResult> CreatePersonalChatAsync(User participant);
    public Task<IEnumerable<Chat>> GetChatsForUserAsync(string email);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName);
    public Task<ChatResult> GetChatByNameAsync(string name);
    public Task<bool> IsChatExistsAsync(string chatId);
    public Task<BaseResult> AddMemberToChatAsync(int chatId, int userId);
    
    
}