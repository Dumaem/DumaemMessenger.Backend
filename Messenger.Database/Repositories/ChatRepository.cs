using Dapper;
using Messenger.Database.Models;
using Messenger.Database.Read;
using Messenger.Database.Read.Queries;
using Messenger.Database.Write;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;

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

    public async Task<DatabaseCreateResult> CreateChatAsync(IEnumerable<User> participants)
    {
        var chat = new ChatDb
        {
            Name = Guid.NewGuid().ToString()
        };

        foreach (var user in participants)
        {
            _context.UserChats.Add(new UserChatDb
            {
                Chat = chat, UserId = user.Id
            });
        }

        await _context.SaveChangesAsync();
        return new DatabaseCreateResult { Success = true, ObjectId = chat.Id, Message = chat.Name };
    }

    public async Task<IEnumerable<Chat>> GetChatsForUserAsync(string email)
    {
        var chats = await _readonlyContext.Connection
            .QueryAsync<ChatDb>(ChatRepositoryQueries.GetChatsForUserAsync, new { email });

        return chats.Select(x => new Chat
        {
            Id = x.Id, Name = x.Name
        });
    }

    public async Task<IEnumerable<User>> GetChatParticipantsAsync(string chatName)
    {
        var users = await _readonlyContext.Connection
            .QueryAsync<UserDb>(ChatRepositoryQueries.GetChatParticipants, new { chatName });

        return users.Select(x => new User
        {
            Email = x.Email, Name = x.Name, Username = x.Username, Id = x.Id
        });
    }
}