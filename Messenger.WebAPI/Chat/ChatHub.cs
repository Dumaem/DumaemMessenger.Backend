using Messenger.Domain.Enums;
using Messenger.Domain.Exception;
using Messenger.Domain.Models;
using Messenger.Domain.Results;
using Messenger.Domain.Services;
using Messenger.WebAPI.Shared.SharedModels;
using Messenger.WebAPI.Shared.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.WebAPI.Chat;

[Authorize]
public class ChatHub : Hub
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IUserService userService,
        IChatService chatService,
        ILogger<ChatHub> logger,
        IMessageService messageService)
    {
        _userService = userService;
        _chatService = chatService;
        _logger = logger;
        _messageService = messageService;
    }

    public override async Task OnConnectedAsync()
    {
        var user = await GetUserFromContextAsync();
        var userChats = await _chatService.GetChatsForUserAsync(user.Email);
        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.Name);
            await Clients.Group(chat.Name).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext { Status = UserOnlineStatus.Online, UserId = user.Id });
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await GetUserFromContextAsync();
        var chats = await _chatService.GetChatsForUserAsync(user.Email);
        foreach (var chat in chats)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Name);
            await Clients.Group(chat.Name).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext { Status = UserOnlineStatus.Offline, UserId = user.Id });
        }

        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName(SignalRServerMethods.SendMessage)]
    public async Task SendMessageToChat(MessageContext message, SendMessageOptions[] options)
    {
        var user = await GetUserFromContextAsync();
        if (!await _chatService.IsChatExistsAsync(message.ChatId))
        {
            var result = new BaseResult { Success = false, Message = "Passed chat does not exist" };
            await Clients.Caller.SendAsync(SignalRClientMethods.MessageNotDelivered, result);
            return;
        }

        var domainMessage = new Message
        {
            ForwardedMessageId = message.ForwardedMessageId, RepliedMessageId = message.RepliedMessageId,
            Content = new MessageContent { Content = message.Content, TypeId = message.ContentType },
            SenderId = user.Id, DateOfDispatch = DateTime.Now
        };
        var res = await _messageService.SaveMessageAsync(domainMessage, message.ChatId, options);
        if (!res.Success)
        {
            var result = new BaseResult { Success = false, Message = "Error creating a message: " + res.Message };
            await Clients.Caller.SendAsync(SignalRClientMethods.MessageNotDelivered, result);
            return;
        }

        var entity = res.Entity;
        message = new MessageContext
        {
            ContentType = entity.Content.TypeId,
            Content = entity.Content.Content,
            UserId = user.Id,
            ForwardedMessageId = entity.ForwardedMessageId,
            RepliedMessageId = entity.RepliedMessageId,
            SendDate = entity.DateOfDispatch,
            ChatId = message.ChatId
        };
        await Clients.Group(message.ChatId).SendAsync(SignalRClientMethods.ReceiveMessage, message);
    }

    private async Task<User> GetUserFromContextAsync()
    {
        var email = Context.UserIdentifier;
        if (email is null)
            throw new ChatException(ChatExceptionType.WrongToken);

        var user = await _userService.GetUserByEmailAsync(email);
        if (user is null)
            throw new ChatException(ChatExceptionType.WrongToken);

        return user;
    }
}