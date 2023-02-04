using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services.Impl;

public class ChatService : IChatService
{
    private IChatRepository _repository;

    public ChatService(IChatRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResult> CreateChatAsync(IEnumerable<User> participants)
    {
        return await _repository.CreateChatAsync(participants);
    }

    public async Task<IEnumerable<Chat>> GetChatsForUserAsync(string email)
    {
        return await _repository.GetChatsForUserAsync(email);
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName)
    {
        return await _repository.GetChatParticipantsAsync(chatName);
    }
}