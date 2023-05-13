using Messenger.Domain.Models;
using Messenger.Domain.Results;

namespace Messenger.Domain.Repositories;

public interface IMessageRepository
{
    Task<ListDataResult<Message>> ListMessagesAsync(string chatId, int count, int offset);
    Task<long> CreateMessageAsync(Message message, string chatId);
    Task<Message> GetMessageByIdAsync(long id);
    Task EditMessageByIdAsync(long id, Message editedMessage);
    Task DeleteMessageForUserAsync(long deletedMessageId, int userId);
    Task DeleteMessageForAllUsers(long deletedMessageId);
    Task CreateReadMessage(long messageId, int userId);
    Task<string> GetShortMessagePreview(long messageId);
}