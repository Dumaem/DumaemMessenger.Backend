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
    private readonly IUserRepository _userRepository;

    public ChatRepository(MessengerContext context, MessengerReadonlyContext readonlyContext,
        IUserRepository userRepository)
    {
        _context = context;
        _readonlyContext = readonlyContext;
        _userRepository = userRepository;
    }

    public async Task<EntityResult<Chat>> CreateChatAsync(IEnumerable<int> participants, string? groupName = null)
    {
        var userIds = participants as int[] ?? participants.ToArray();
        var chat = new ChatDb
        {
            Guid = Guid.NewGuid().ToString(),
            GroupName = groupName,
            IsPersonal = userIds.Length == 2,
        };

        foreach (var userId in userIds)
        {
            _context.UserChats.Add(new UserChatDb
            {
                Chat = chat, UserId = userId
            });
        }

        await _context.SaveChangesAsync();
        return new EntityResult<Chat>()
        {
            Success = true,
            Entity = new Chat
                { GroupName = chat.GroupName, Name = chat.Guid, IsPersonal = chat.IsPersonal, Id = chat.Id },
            Message = chat.Guid
        };
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
        var userChats = _context.Chats
            .Include(x => x.Users)
            .Where(x => userChatIds.Contains(x.Id));
        var res = new List<ChatResult>();
        foreach (var c in userChats)
        {
            var currentChat = new ChatResult
            {
                Success = true, ChatId = c.Id,
                ChatName = c.IsPersonal
                    ? (await GetChatParticipantsAsync(c.Id)).First(x => x.Id != id).Name
                    : c.GroupName!,
                ChatGuid = c.Guid,
            };
            var lastMessage = _context.Messages
                .Include(x => x.MessageContent)
                .Where(x => x.ChatId == c.Id && !x.IsDeleted && x.DeletedMessages.All(z => z.UserId != id))
                .OrderBy(x => x.DateOfDispatch)
                .LastOrDefault();
            if (lastMessage is not null)
            {
                currentChat.LastMessage = EntityConverter.ConvertMessage(lastMessage);
                currentChat.SenderName = (await _userRepository.GetUserByIdAsync(lastMessage.SenderId))?.Name;
            }

            res.Add(currentChat);
        }

        return res;
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string name)
    {
        var users = await _readonlyContext.Connection
            .QueryAsync<UserDb>(ChatRepositoryQueries.GetChatParticipantsByName, new { name });

        return users.Select(x => new User
        {
            Email = x.Email, Name = x.Name, Username = x.Username, Id = x.Id
        });
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(int chatId)
    {
        var users = await _readonlyContext.Connection
            .QueryAsync<UserDb>(ChatRepositoryQueries.GetChatParticipantsById, new { chatId });

        return users.Select(x => new User
        {
            Email = x.Email, Name = x.Name, Username = x.Username, Id = x.Id
        });
    }

    public async Task<Chat?> GetChatByName(string name, int currentUserId)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<ChatDb>(
            ChatRepositoryQueries.GetChatByName, new { name });
        return res is null
            ? null
            : new Chat
            {
                Id = res.Id, Name = res.Guid, IsPersonal = res.IsPersonal, GroupName = res.IsPersonal
                    ? (await GetChatParticipantsAsync(res.Id)).First(x => x.Id != currentUserId).Name
                    : res.GroupName,
                ParticipantCount = _context.UserChats.Count(x => x.ChatId == res.Id)
            };
    }

    public async Task<Chat?> GetChatById(int id, int currentUserId)
    {
        var res = await _readonlyContext.Connection.QuerySingleOrDefaultAsync<ChatDb>(
            ChatRepositoryQueries.GetChatById,
            new { id });
        return res is null
            ? null
            : new Chat
            {
                Id = res.Id, Name = res.Guid, IsPersonal = res.IsPersonal, GroupName = res.IsPersonal
                    ? (await GetChatParticipantsAsync(res.Id)).First(x => x.Id != currentUserId).Name
                    : res.GroupName,
                ParticipantCount = _context.UserChats.Count(x => x.ChatId == id)
            };
    }

    public async Task<bool> IsChatExistsAsync(string chatId)
    {
        return await _readonlyContext.Connection.ExecuteScalarAsync<bool>(ChatRepositoryQueries.IsChatExists,
            new { chatId });
    }

    public async Task<ListDataResult<int>> AddMembersToChat(string chatGuid, IEnumerable<int> userIds)
    {
        var chatId = (await _context.Chats.FirstOrDefaultAsync(x => x.Guid == chatGuid))?.Id ??
                     throw new NotFoundException();

        var addedUsers = new List<int>();
        var usersArray = userIds as int[] ?? userIds.ToArray();
        foreach (var userId in usersArray)
        {
            var canAddUser = _context.Users.Any(x => x.Id == userId)
                             && !_context.UserChats.Any(x => x.ChatId == chatId && x.UserId == userId);
            if (!canAddUser)
                continue;

            _context.UserChats.Add(new UserChatDb
            {
                UserId = userId,
                ChatId = chatId
            });
            addedUsers.Add(userId);
        }

        await _context.SaveChangesAsync();

        var ignoredUsers = usersArray.Except(addedUsers);
        return new ListDataResult<int>
        {
            Success = true, Items = addedUsers,
            Message = $"Following users were failed to add: {string.Join(", ", ignoredUsers)}"
        };
    }

    public async Task<bool> IsMemberParted(string chatGuid, int memberId)
    {
        var chatId = (await _context.Chats.FirstOrDefaultAsync(x => x.Guid == chatGuid))?.Id ??
                     throw new NotFoundException();
        return await _context.UserChats.FirstOrDefaultAsync(x => x.ChatId == chatId && x.UserId == memberId) != null;
    }
}