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

    public async Task<BaseResult> CreateChatAsync(IEnumerable<int> participants, string groupName,
        int currentUserId)
    {
        return await _repository.CreateChatAsync(participants, false, groupName, currentUserId);
    }

    public async Task<BaseResult> CreatePersonalChatAsync(int participantId, int currentUserId)
    {
        var participants = new List<int>{participantId};
        return await _repository.CreateChatAsync(participants, true, null, currentUserId);
    }

    public async Task<IEnumerable<ChatResult>> GetChatsForUserAsync(string email)
    {
        return await _repository.GetChatsForUserAsync(email);
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName)
    {
        return await _repository.GetChatParticipantsAsync(chatName);
    }

    public async Task<EntityResult<Chat>> GetChatByNameAsync(string name)
    {
        var res = await _repository.GetChatByName(name);
        return res is null ? 
            new EntityResult<Chat>{Message = string.Format(ChatErrorMessages.ChatWithNameNotFound, name)} 
            : new EntityResult<Chat>{Entity = res, Success = true};
    }

    public async Task<bool> IsChatExistsAsync(string chatId)
    {
        return await _repository.IsChatExistsAsync(chatId);
    }

    public async Task<BaseResult> AddMemberToChatAsync(int chatId, int userId)
    {
        return await _repository.AddMemberToChat(chatId, userId);
    }
}