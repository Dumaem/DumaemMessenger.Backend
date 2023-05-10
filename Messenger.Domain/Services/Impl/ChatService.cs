using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services.Impl;

public class ChatService : IChatService
{
    private readonly IChatRepository _repository;

    public ChatService(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResult> CreateChatAsync(IEnumerable<User> participants)
    {
        return await _repository.CreateChatAsync(participants);
    }

    public async Task<BaseResult> CreatePersonalChatAsync(User participant)
    {
        return await _repository.CreatePersonalChatAsync(participant);
    }

    public async Task<IEnumerable<Chat>> GetChatsForUserAsync(string email)
    {
        return await _repository.GetChatsForUserAsync(email);
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName)
    {
        return await _repository.GetChatParticipantsAsync(chatName);
    }

    public async Task<ChatResult> GetChatByNameAsync(string name)
    {
        var res = await _repository.GetChatByName(name);
        return res is null ? 
            new ChatResult{Message = string.Format(ChatErrorMessages.ChatWithNameNotFound, name)} 
            : new ChatResult{Chat = res, Success = true};
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