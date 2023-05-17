using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services.Impl;

public class ChatService : IChatService
{
    private readonly IChatRepository _repository;
    private readonly IUserRepository _userRepository;

    public ChatService(IChatRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<EntityResult<Chat>> CreateChatAsync(IEnumerable<int> participantIds, string? groupName = null)
    {
        var participants = participantIds as int[] ?? participantIds.ToArray();
        if (participants.Length == 2) groupName = default;
        return await _repository.CreateChatAsync(participants, groupName);
    }

    public async Task<IEnumerable<ChatResult>> GetChatsForUserAsync(string email)
    {
        return await _repository.GetChatsForUserAsync(email);
    }

    public async Task<IEnumerable<ChatResult>> GetChatsForUserAsync(int id)
    {
        return await _repository.GetChatsForUserAsync(id);
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName)
    {
        return await _repository.GetChatParticipantsAsync(chatName);
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(int chatId)
    {
        return await _repository.GetChatParticipantsAsync(chatId);
    }

    public async Task<EntityResult<Chat>> GetChatAsync(string name, int currentUserId)
    {
        var res = await _repository.GetChatByName(name, currentUserId);
        return res is null
            ? new EntityResult<Chat> { Message = string.Format(ChatErrorMessages.ChatWithNameNotFound, name) }
            : new EntityResult<Chat> { Entity = res, Success = true };
    }

    public async Task<EntityResult<Chat>> GetChatAsync(int id, int currentUserId)
    {
        var res = await _repository.GetChatById(id, currentUserId);
        return res is null
            ? new EntityResult<Chat> { Message = string.Format(ChatErrorMessages.ChatWithNameNotFound, id) }
            : new EntityResult<Chat> { Entity = res, Success = true };
    }

    public async Task<bool> IsChatExistsAsync(string chatId)
    {
        return await _repository.IsChatExistsAsync(chatId);
    }

    public async Task<bool> IsMemberParted(string chatGuid, int memberId)
    {
        return await _repository.IsMemberParted(chatGuid, memberId);
    }

    public async Task<ListDataResult<int>> AddMembersToChatAsync(string chatGuid, IEnumerable<int> userIds)
    {
        return await _repository.AddMembersToChat(chatGuid, userIds);
    }
}