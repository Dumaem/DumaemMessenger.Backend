using Dapper;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Exception;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Results;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly MessengerContext _context;
    private readonly MessengerReadonlyContext _readonlyContext;

    public ChatRepository(MessengerContext context, MessengerReadonlyContext readonlyContext)
    {
        _context = context;
        _readonlyContext = readonlyContext;
    }

    public async Task<DatabaseCreateResult> CreateChatAsync(IEnumerable<int> participants, bool isPersonal,
        string? groupName, int currentUserId)
    {
        var chat = new ChatDb
        {
            Name = Guid.NewGuid().ToString(),
            GroupName = groupName,
            IsPersonal = isPersonal,
        };

        foreach (var userId in participants)
        {
            _context.UserChats.Add(new UserChatDb
            {
                Chat = chat, UserId = userId
            });
        }

        _context.UserChats.Add(new UserChatDb
        {
            Chat = chat, UserId = currentUserId 
        });

        await _context.SaveChangesAsync();
        return new DatabaseCreateResult {Success = true, ObjectId = chat.Id, Message = chat.Name};
    }

    public async Task<IEnumerable<ChatResult>> GetChatsForUserAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email) ?? throw new NotFoundException();
        return await GetChatsForUserAsync(user.Id);
    }

    public async Task<IEnumerable<ChatResult>> GetChatsForUserAsync(int id)
    {
        var userChatIds = _context.UserChats.Where(x => x.UserId == id)
            .Select(x => x.ChatId);
        var userChats = _context.Chats.Include(x => x.Users)
            .Include(x => x.Messages).ThenInclude(x=>x.MessageContent)
            .Where(x => userChatIds.Contains(x.Id));
        var res = new List<ChatResult>();
        foreach (var c in userChats)
        {
            var chatName = (await GetChatParticipantsAsync(c.Id)).First(x => x.Id != id).Name;
            var lastMessage = c.Messages.LastOrDefault();
            if (lastMessage is null)
            {
                res.Add(new ChatResult
                {
                    Success = true,
                    ChatId = c.Id,
                    ChatName = c.IsPersonal 
                        ? c.GroupName = chatName : c.GroupName!,
                    LastMessage = null
                });
            }

            else
            {
                var convertedLastMessage = EntityConverter.ConvertMessage(lastMessage);
                convertedLastMessage.Content = EntityConverter.ConvertMessageContent(lastMessage.MessageContent);
                res.Add(new ChatResult
                {
                    Success = true,
                    ChatId = c.Id,
                    ChatName = c.IsPersonal ? chatName : c.GroupName!,
                    LastMessage = convertedLastMessage,
                });
            }
        }

        return res;
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string name)
    {
        var users = await _readonlyContext.Connection
            .QueryAsync<UserDb>(ChatRepositoryQueries.GetChatParticipantsByName, new {name});

        return users.Select(x => new User
        {
            Email = x.Email, Name = x.Name, Username = x.Username, Id = x.Id
        });
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(int chatId)
    {
        var users = await _readonlyContext.Connection
            .QueryAsync<UserDb>(ChatRepositoryQueries.GetChatParticipantsById, new {chatId});

        return users.Select(x => new User
        {
            Email = x.Email, Name = x.Name, Username = x.Username, Id = x.Id
        });
    }

    public async Task<Chat?> GetChatByName(string name, int currentUserId)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<ChatDb>(
            ChatRepositoryQueries.GetChatByName, new {name});
        return res is null
            ? null
            : new Chat
            {
                Id = res.Id, Name = res.Name, IsPersonal = res.IsPersonal, GroupName = res.IsPersonal 
                    ? (await GetChatParticipantsAsync(res.Id)).First( x=> x.Id != currentUserId).Name : res.GroupName
            };
    }

    public async Task<Chat?> GetChatById(int id, int currentUserId)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<ChatDb>(
            ChatRepositoryQueries.GetChatById,
            new {id});
        return res is null
            ? null
            : new Chat
            {
                Id = res.Id, Name = res.Name, IsPersonal = res.IsPersonal, GroupName = res.IsPersonal 
                    ? (await GetChatParticipantsAsync(res.Id)).First( x=> x.Id != currentUserId).Name : res.GroupName
            };
    }

    public async Task<bool> IsChatExistsAsync(string chatId)
    {
        return await _readonlyContext.Connection.ExecuteScalarAsync<bool>(ChatRepositoryQueries.IsChatExists,
            new {chatId});
    }

    public async Task<BaseResult> AddMemberToChat(int chatId, int userId)
    {
        _context.UserChats.Add(new UserChatDb
        {
            UserId = userId,
            ChatId = chatId
        });
        await _context.SaveChangesAsync();

        return new BaseResult {Success = true};
    }
}