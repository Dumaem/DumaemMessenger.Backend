using Messenger.Domain.Exception;
using Messenger.Domain.Models;
using Messenger.Domain.Services;
using Messenger.WebAPI.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Chat;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IUserService userService, IChatService chatService, ILogger<ChatHub> logger)
    {
        _userService = userService;
        _chatService = chatService;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var user = await GetUserAsync();
        var userChats = await _chatService.GetChatsForUserAsync(user.Email);
        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Name);
            await Clients.Group(chat.Name).SendAsync("BecameOnline", "");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userChats = await GetChatsForUserAsync();
        foreach (var chat in userChats)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Name);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToChat(MessageContext message)
    {
        await Clients.Group(message.ChatId).SendAsync("ReceiveMessage", message);
    }

    private async Task<User> GetUserAsync()
    {
        var email = Context.UserIdentifier;
        if (email is null)
            throw new ChatException(ChatExceptionType.WrongToken);

        var user = await _userService.GetUserByEmailAsync(email);
        if (user is null)
            throw new ChatException(ChatExceptionType.WrongToken);

        return user;
    }

    private async Task<IEnumerable<Domain.Models.Chat>> GetChatsForUserAsync()
    {
        var user = await GetUserAsync();
        return await _chatService.GetChatsForUserAsync(user.Email);
    }
}