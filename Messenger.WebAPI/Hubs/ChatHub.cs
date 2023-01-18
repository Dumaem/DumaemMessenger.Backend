using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Hubs;

public class ChatHub : Hub
{
    public Task SendMessage(string message)
    {
        var userId = Context.User.Identity.Name;
        return Clients.Others.SendAsync("Send", message);
    }
}