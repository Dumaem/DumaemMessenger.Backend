using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IChatService
{
    public Task<BaseResult> CreateChatAsync(IEnumerable<int> participants, string groupName,
        int currentUserId);
    public Task<BaseResult> CreatePersonalChatAsync(int participant,int currentUser);
    public Task<IEnumerable<ChatResult>> GetChatsForUserAsync(string email);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName);
    public Task<EntityResult<Chat>> GetChatByNameAsync(string name);
    public Task<bool> IsChatExistsAsync(string chatId);
    public Task<BaseResult> AddMemberToChatAsync(int chatId, int userId);
    
    
}