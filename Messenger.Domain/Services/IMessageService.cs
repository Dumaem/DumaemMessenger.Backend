using Messenger.Domain.Enums;
using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Services;

public interface IMessageService
{
    Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int count, int offset);
    Task<EntityResult<Message>> SaveMessageAsync(Message message, string chatId, SendMessageOptions[] options);
    Task<EntityResult<Message>> EditMessageAsync(Message message);
    Task<BaseResult> ReadMessageAsync(long messageId, int userId);
    Task<string> GetChatNameFromMessage(long messageId);
    Task<string> GetShortMessagePreview(long messageId);
    // Task DeleteMessageAsync(long messageId, int? userId = null);
}