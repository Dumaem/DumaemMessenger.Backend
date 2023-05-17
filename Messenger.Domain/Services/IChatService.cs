using System.Collections;
using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IChatService
{
    public Task<EntityResult<Chat>> CreateChatAsync(IEnumerable<int> participantIds, string? groupName = null);
    public Task<IEnumerable<ChatResult>> GetChatsForUserAsync(string email);
    public Task<IEnumerable<ChatResult>> GetChatsForUserAsync(int id);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(int chatId);
    public Task<EntityResult<Chat>> GetChatAsync(string name, int currentUserId);
    public Task<EntityResult<Chat>> GetChatAsync(int id, int currentUserId);
    public Task<bool> IsChatExistsAsync(string chatId);
    public Task<bool> IsMemberParted(string chatGuid, int memberId);
    public Task<ListDataResult<int>> AddMembersToChatAsync(string chatGuid, IEnumerable<int> userIds);
    
    
}