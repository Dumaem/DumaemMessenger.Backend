using Messenger.Domain.Services;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private IUserService _userService;

    public ChatHub(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task OnConnectedAsync()
    {
        var email = Context.UserIdentifier;
        if (email is null)
            return;

        var user = await _userService.GetUserByEmailAsync(email);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public Task SendMessageToChat(MessageContext message)
    {
        return Clients.Others.SendAsync("Send", message);
    }
}