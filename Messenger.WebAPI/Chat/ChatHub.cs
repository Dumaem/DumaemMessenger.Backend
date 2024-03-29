﻿using System.Collections.Concurrent;
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

    private static readonly ConcurrentDictionary<int, List<string>> UserConnections = new();

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
        UserConnections.GetOrAdd(user.Id, new List<string>()).Add(Context.ConnectionId);

        var userChats = await _chatService.GetChatsForUserAsync(user.Email);
        foreach (var chat in userChats)
        {
            _logger.LogInformation("User {Username} connected to chat {ChatName}", user.Username, chat.ChatGuid);
            await Groups.AddToGroupAsync(Context.ConnectionId, chat.ChatGuid);
            await Clients.Group(chat.ChatGuid).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext {Status = UserOnlineStatus.Online, UserId = user.Id});
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = await GetUserFromContextAsync();
        if (UserConnections.TryGetValue(user.Id, out var value))
        {
            value.Remove(Context.ConnectionId);
        }

        var chats = await _chatService.GetChatsForUserAsync(user.Email);
        foreach (var chat in chats)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.ChatGuid);
            await Clients.Group(chat.ChatGuid).SendAsync(SignalRClientMethods.StatusChanged,
                new UserStatusContext {Status = UserOnlineStatus.Offline, UserId = user.Id});
            _logger.LogInformation("User {Username} disconnected from chat {ChatName}", user.Username, chat.ChatGuid);
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
        _logger.LogInformation("Message from user {UserName} sent to chat {ChatGuid}", user.Username, message.ChatId);
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

    [HubMethodName(SignalRServerMethods.CreateChat)]
    public async Task CreateChat(int[] participantIds, string? chatName = null)
    {
        var user = await GetUserFromContextAsync();
        if (!participantIds.Contains(user.Id))
        {
            return;
        }

        var result = await _chatService.CreateChatAsync(participantIds, chatName);

        if (!result.Success) return;

        var connections = participantIds.SelectMany(x =>
                UserConnections.TryGetValue(x, out var connections)
                    ? connections
                    : new List<string>())
            .ToArray();
        await Clients.Clients(connections).SendAsync(SignalRClientMethods.ChatCreated, result.Entity);
        foreach (var connection in connections)
        {
            await Groups.AddToGroupAsync(connection, result.Entity.Name);
        }
    }

    [HubMethodName(SignalRServerMethods.AddMembersToChat)]
    public async Task AddMembersToChat(string chatGuid, IEnumerable<int> memberIds)
    {
        var user = await GetUserFromContextAsync();

        if (!await _chatService.IsMemberParted(chatGuid, user.Id))
        {
            return;
        }

        var userId = memberIds as int[] ?? memberIds.ToArray();
        var result = await _chatService.AddMembersToChatAsync(chatGuid, userId);

        if (!result.Success) return;

        var newMembersConnections =
            userId.SelectMany(x => UserConnections.TryGetValue(x, out var value) ? value : new List<string>());
        foreach (var connection in newMembersConnections)
        {
            await Groups.AddToGroupAsync(connection, chatGuid);
        }

        await Clients.Group(chatGuid).SendAsync(SignalRClientMethods.MemberAdded, chatGuid, result.Items);
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