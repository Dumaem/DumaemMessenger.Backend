﻿using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Repositories;

public interface IChatRepository
{
    public Task<DatabaseCreateResult> CreateChatAsync(IEnumerable<int> participants, bool isPersonal,
    string? groupName, int currentUserId);
    public Task<IEnumerable<ChatResult>> GetChatsForUserAsync(string email);
    public Task<IEnumerable<ChatResult>> GetChatsForUserAsync(int id);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName);
    public Task<IEnumerable<User>> GetChatParticipantsAsync(int chatId);
    public Task<Chat?> GetChatByName(string name, int currentUserId);
    public Task<Chat?> GetChatById(int id, int currentUserId);
    public Task<bool> IsChatExistsAsync(string chatId);
    public Task<BaseResult> AddMemberToChat(int chatId, int userId);
}