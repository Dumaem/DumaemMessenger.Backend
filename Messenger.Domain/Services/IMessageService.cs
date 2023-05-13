using Messenger.Domain.Enums;
using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IMessageService
{
    Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int count, int offset);
    Task<EntityResult<Message>> SaveMessageAsync(Message message, string chatId, SendMessageOptions[] options);
    Task<EntityResult<Message>> EditMessageAsync(Message message);
    Task<string> GetShortMessagePreview(long messageId);
    // Task SendMessageAsync(int chatId, int userId, byte[] content, int messageTypeId);
    // Task DeleteMessageAsync(long messageId, int? userId = null);
    // Task ReadMessageAsync(long messageId, int userId);
    // Task ReplyMessageAsync(long repliedMessageId, int repliedMessageChatId,
    //     int newMessageSenderId, byte[] content, int messageTypeId);
    // Task ForwardMessageAsync(long forwardedMessageId, byte[] content, int messageTypeId, int chatId, int userId);
    // Task EditMessageAsync(long messageId, byte[] content, int messageTypeId);
}