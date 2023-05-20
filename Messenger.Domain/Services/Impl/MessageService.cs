using Messenger.Domain.Models;
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

    public async Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int userId, int count, int offset)
    {
        var res = await _messageRepository.ListMessagesAsync(chatId, userId, count, offset);
        return res;
    }

    public async Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int initialCount, int userId, int count,
        int offset)
    {
        var res = await _messageRepository.ListMessagesAsync(chatId, userId, initialCount,
            count, offset);
        return res;
    }

    public async Task<EntityResult<Message>> SaveMessageAsync(Message message, string chatId,
        SendMessageOptions[] options)
    {
        if (options.Contains(SendMessageOptions.ForwardMessage) &&
            options.Contains(SendMessageOptions.ReplyToMessage))
        {
            return new EntityResult<Message>
                {Success = false, Message = "Cannot both reply and forward a message"};
        }

        if (options.Contains(SendMessageOptions.ForwardMessage))
            message.RepliedMessageId = null;
        else
            message.ForwardedMessageId = null;
        var result = await _messageRepository.CreateMessageAsync(message, chatId);
        return new EntityResult<Message>
            {Success = true, Entity = await _messageRepository.GetMessageByIdAsync(result)};
    }

    public async Task<EntityResult<Message>> EditMessageAsync(Message message)
    {
        await _messageRepository.EditMessageByIdAsync(message.Id, message);
        return new EntityResult<Message>
            {Success = true, Entity = await _messageRepository.GetMessageByIdAsync(message.Id)};
    }

    public async Task<BaseResult> ReadMessageAsync(long messageId, int userId)
    {
        await _messageRepository.CreateReadMessage(messageId, userId);
        return new BaseResult {Success = true};
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
}