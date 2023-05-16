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
            _logger.LogWarning("User {Username} connected to chat {ChatName}", user.Username, chat.ChatGuid);
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.ChatGuid);
            await Clients.Group(chat.ChatGuid).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext {Status = UserOnlineStatus.Online, UserId = user.Id});
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await GetUserFromContextAsync();
        var chats = await _chatService.GetChatsForUserAsync(user.Email);
        foreach (var chat in chats)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.ChatGuid);
            await Clients.Group(chat.ChatGuid).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext {Status = UserOnlineStatus.Offline, UserId = user.Id});
            _logger.LogWarning("User {Username} disconnected from chat {ChatName}", user.Username, chat.ChatGuid);
        }

        await base.OnDisconnectedAsync(exception);
    }

    [HubMethodName(SignalRServerMethods.SendMessage)]
    public async Task SendMessageToChat(MessageContext message)
    {
        var user = await GetUserFromContextAsync();
        _logger.LogInformation("Message received from client {ClientName}", user.Name);
        if (!await _chatService.IsChatExistsAsync(message.ChatId))
        {
            var result = new BaseResult {Success = false, Message = "Passed chat does not exist"};
            await Clients.Caller.SendAsync(SignalRClientMethods.MessageNotDelivered, result);
            return;
        }

        var domainMessage = new Message
        {
            ForwardedMessageId = message.ForwardedMessageId, RepliedMessageId = message.RepliedMessageId,
            Content = new MessageContent {Content = message.Content, TypeId = message.ContentType},
            SenderId = user.Id, DateOfDispatch = DateTime.Now
        };
        var res = await _messageService.SaveMessageAsync(domainMessage, message.ChatId,
            Array.Empty<SendMessageOptions>());
        if (!res.Success)
        {
            var result = new BaseResult {Success = false, Message = "Error creating a message: " + res.Message};
            await Clients.Caller.SendAsync(SignalRClientMethods.MessageNotDelivered, result);
            return;
        }

        var entity = res.Entity;
        message = new MessageContext
        {
            Id = entity.Id,
            ContentType = entity.Content.TypeId,
            Content = entity.Content.Content,
            UserId = user.Id,
            ForwardedMessageId = entity.ForwardedMessageId,
            RepliedMessageId = entity.RepliedMessageId,
            SendDate = entity.DateOfDispatch,
            ChatId = message.ChatId,
            User = user,
        };
        _logger.LogWarning("Message from user {UserName} sent to chat {ChatGuid}", user.Username, message.ChatId);
        await Clients.Group(message.ChatId).SendAsync(SignalRClientMethods.ReceiveMessage, message);
    }

    [HubMethodName(SignalRServerMethods.EditMessage)]
    public async Task EditMessage(EditMessageContext message)
    {
        var user = await GetUserFromContextAsync();
        if (!await _chatService.IsChatExistsAsync(message.ChatId))
        {
            var result = new BaseResult {Success = false, Message = "Passed chat does not exist"};
            await Clients.Caller.SendAsync(SignalRClientMethods.MessageNotDelivered, result);
            return;
        }

        var domainMessage = new Message
        {
            Content = new MessageContent {Content = message.Content, TypeId = message.ContentType},
            SenderId = user.Id, Id = message.MessageId
        };

        var res = await _messageService.EditMessageAsync(domainMessage);
        if (!res.Success)
        {
            var result = new BaseResult {Success = false, Message = "Error creating a message: " + res.Message};
            await Clients.Caller.SendAsync(SignalRClientMethods.MessageNotDelivered, result);
            return;
        }

        var entity = res.Entity;
        message = new EditMessageContext
        {
            ContentType = entity.Content.TypeId,
            Content = entity.Content.Content,
            ChatId = message.ChatId,
            MessageId = res.Entity.Id,
        };
        await Clients.Group(message.ChatId).SendAsync(SignalRClientMethods.MessageEdited, message);
    }

    [HubMethodName(SignalRServerMethods.ReadMessage)]
    public async Task ReadMessage(long messageId)
    {
        var user = await GetUserFromContextAsync();

        var res = await _messageService.ReadMessageAsync(messageId, user.Id);
        if (!res.Success)
        {
            return;
        }

        var chatName = await _messageService.GetChatNameFromMessage(messageId);
        var context = new MessageReadContext {UserId = user.Id, MessageId = messageId, ChatName = chatName};
        await Clients.Group(chatName).SendAsync(SignalRClientMethods.MessageRead, context);
    }

    [HubMethodName(SignalRServerMethods.DeleteMessage)]
    public async Task DeleteMessage(long messageId)
    {
        await _messageService.DeleteMessageAsync(messageId);
        var chatName = await _messageService.GetChatNameFromMessage(messageId);
        await Clients.Group(chatName).SendAsync(SignalRClientMethods.MessageDeleted, messageId);
    }

    private async Task<User> GetUserFromContextAsync()
    {
        var email = Context.UserIdentifier;
        if (email is null)
            throw new ChatException(ChatExceptionType.WrongToken);

        var user = await _userService.GetUserAsync(email);
        if (user is null)
            throw new ChatException(ChatExceptionType.WrongToken);

        return user;
    }
}