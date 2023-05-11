using Messenger.Domain.Enums;
using Messenger.Domain.Exception;
using Messenger.Domain.Models;
using Messenger.Domain.Results;
using Messenger.Domain.Services;
using Messenger.WebAPI.Shared.Client;
using Messenger.WebAPI.Shared.SharedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Chat;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;
    private readonly ILogger<ChatHub> _logger;
    private User _user = null!;

    public ChatHub(IUserService userService, IChatService chatService, ILogger<ChatHub> logger)
    {
        _userService = userService;
        _chatService = chatService;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _user = await GetUserAsync();
        var userChats = await _chatService.GetChatsForUserAsync(_user.Email);
        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Entity.Name);
            await Clients.Group(chat.Entity.Name).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext { Status = UserOnlineStatus.Online, UserId = _user.Id });
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var chats = await GetChatsForUserAsync();
        foreach (var chat in chats)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Entity.Name);
            await Clients.Group(chat.Entity.Name).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext { Status = UserOnlineStatus.Offline, UserId = _user.Id });
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessageToChat(MessageContext message)
    {
        if (!await _chatService.IsChatExistsAsync(message.ChatId))
        {
            var result = new BaseResult { Success = true, Message = "Passed chat does not exits" };
            await Clients.Caller.SendAsync(SignalRClientMethods.MessageNotDelivered, result);
            return;
        }
        await Clients.Group(message.ChatId).SendAsync(SignalRClientMethods.ReceiveMessage, message);
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

    private async Task<IEnumerable<ChatResult>> GetChatsForUserAsync()
    {
        return await _chatService.GetChatsForUserAsync(_user.Email);
    }
}