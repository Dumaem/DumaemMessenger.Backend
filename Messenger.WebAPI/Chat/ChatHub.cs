using Messenger.Domain.ErrorMessages;
using Messenger.Domain.Exception;
using Messenger.Domain.Services;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Chat;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;

    public ChatHub(IUserService userService, IChatService chatService)
    {
        _userService = userService;
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        var userChats = await GetChatsForUserAsync();
        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Name);
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

    public Task SendMessageToChat(MessageContext message)
    {
        return Clients.Others.SendAsync("Send", message);
    }

    private async Task<IEnumerable<Domain.Models.Chat>> GetChatsForUserAsync()
    {
        var email = Context.UserIdentifier;
        if (email is null)
        {
            Context.Abort();
            throw new ChatException(ServerErrorMessages.TokenDoesNotBelongToUser);
        }

        var user = await _userService.GetUserByEmailAsync(email);
        if (user is null)
        {
            Context.Abort();
            throw new ChatException(ServerErrorMessages.TokenDoesNotBelongToUser);
        }

        return await _chatService.GetChatsForUserAsync(user.Email);
    }
}