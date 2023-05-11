using Dapper;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
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
        string? groupName)
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

        await _context.SaveChangesAsync();
        return new DatabaseCreateResult {Success = true, ObjectId = chat.Id, Message = chat.Name};
    }

    public async Task<IEnumerable<ChatResult>> GetChatsForUserAsync(string email)
    {
        var user = _context.Users.First(x => x.Email == email);
        var userChatIds = _context.UserChats.Where(x => x.UserId == user.Id)
            .Select(x => x.ChatId);
        var userChats = _context.Chats.Include(x => x.Users)
            .Include(x => x.Messages)
            .Where(x => userChatIds.Contains(x.Id));
        var res = new List<ChatResult>();
        foreach (var s in userChats)
        {
            var lastMessage = s.Messages.Last();
            var chat = new ChatResult
            {
                ChatId = s.Id,
                ChatName = s.GroupName!,
                LastMessage = new Message
                {
                    Id = lastMessage.Id,
                    DateOfDispatch = lastMessage.DateOfDispatch,
                    IsEdited = lastMessage.IsEdited,
                    IsDeleted = lastMessage.IsDeleted,
                    SenderId = lastMessage.SenderId,
                    ChatId = lastMessage.ChatId,
                    RepliedMessageId = lastMessage.RepliedMessageId,
                    ForwardedMessageId = lastMessage.ForwardedMessageId,
                    Content = lastMessage.MessageContents
                },
            };
        }
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName)
    {
        var users = await _readonlyContext.Connection
            .QueryAsync<UserDb>(ChatRepositoryQueries.GetChatParticipants, new {chatName});

        return users.Select(x => new User
        {
            Email = x.Email, Name = x.Name, Username = x.Username, Id = x.Id
        });
    }

    public async Task<Chat?> GetChatByName(string name)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<ChatDb>(
            ChatRepositoryQueries.GetChatByName,
            new {name});
        return res is null
            ? null
            : new Chat
            {
                Id = res.Id, Name = res.Name, IsPersonal = res.IsPersonal
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