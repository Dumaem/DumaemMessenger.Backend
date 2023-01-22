using System.Text.Json;
using Messenger.WebAPI.Credentials;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Hubs;

public class ChatHub : Hub
{
    public Task SendMessage(MessageContext message)
    {
        string userId = Context.UserIdentifier;
        var auth = Context.Items["Headers"];
        var http = Context.GetHttpContext();
        var headers = http?.Request.Headers;
        return Clients.Others.SendAsync("Send", message);
    }
}