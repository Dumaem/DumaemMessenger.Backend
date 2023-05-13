﻿using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Messenger.Domain.Enums;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services.Impl;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public async Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int count, int offset)
    {
        var res = await _messageRepository.ListMessagesAsync(chatId, count, offset);
        return res;
    }

    public async Task<EntityResult<Message>> SaveMessageAsync(Message message, string chatId,
        SendMessageOptions[] options)
    {
        if (options.Contains(SendMessageOptions.ForwardMessage) &&
            options.Contains(SendMessageOptions.ReplyToMessage))
        {
            return new EntityResult<Message>
                { Success = false, Message = "Cannot both reply and forward a message" };
        }

        if (options.Contains(SendMessageOptions.ForwardMessage))
            message.RepliedMessageId = null;
        else
            message.ForwardedMessageId = null;
        var result = await _messageRepository.CreateMessageAsync(message, chatId);
        return new EntityResult<Message>
            { Success = true, Entity = await _messageRepository.GetMessageByIdAsync(result) };
    }

    public async Task<EntityResult<Message>> EditMessageAsync(Message message)
    {
        await _messageRepository.EditMessageByIdAsync(message.Id, message);
        return new EntityResult<Message>
            { Success = true, Entity = await _messageRepository.GetMessageByIdAsync(message.Id) };
    }

    public async Task<BaseResult> ReadMessageAsync(long messageId, int userId)
    {
        await _messageRepository.CreateReadMessage(messageId, userId);
        return new BaseResult { Success = true };
    }

    public async Task DeleteMessageAsync(long messageId, int? userId = null)
    {
        if (userId is null)
        {
            await _messageRepository.DeleteMessageForAllUsers(messageId);
            return;
        }

        await _messageRepository.DeleteMessageForUserAsync(messageId, userId.Value);
    }

    public async Task<string> GetChatNameFromMessage(long messageId)
    {
        return await _messageRepository.GetChatNameFromMessage(messageId);
    }

    public async Task<string> GetShortMessagePreview(long messageId)
    {
        return await _messageRepository.GetShortMessagePreview(messageId);
    }

    // public async Task SendMessageAsync(int chatId, int userId, byte[] content, int messageTypeId)
    // {
    //     // Use the repository to persist the message to the data store
    //
    //     var message = new Message
    //     {
    //         ChatId = chatId,
    //         SenderId = userId,
    //         Content = new MessageContent { Content = content, TypeId = messageTypeId },
    //         DateOfDispatch = DateTime.UtcNow
    //     };
    //     message.Content.MessageId = message.Id;
    //     await _messageRepository.CreateMessageAsync(message);
    // }
    //
    //
    //     // Use the repository to delete the message for one user
    //     await _messageRepository.DeleteMessageForUserAsync(messageId, (int)userId);
    // }
    //
    // public async Task ReadMessageAsync(long messageId, int userId)
    // {
    //     // Use the repository to mark the message as read
    //     await _messageRepository.CreateReadMessage(messageId, userId);
    // }
    // public async Task EditMessageAsync(long messageId, byte[] content, int messageTypeId)
    // {
    //     var message = await _messageRepository.GetMessageByIdAsync(messageId);
    //     // Use the repository to update the message's content
    //     message.Content = new MessageContent { Content = content, TypeId = messageTypeId };
    //     await _messageRepository.EditMessageByIdAsync(messageId, message);
    // }
}