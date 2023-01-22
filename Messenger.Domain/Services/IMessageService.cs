using Messenger.Domain.Models;

namespace Messenger.Domain.Services
{
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(int chatId, int userId, byte[] content, int messageTypeId);
        Task<bool> DeleteMessageAsync(long messageId, int? userId = null);
        Task<bool> ReadMessageAsync(long messageId, int userId);
        Task<bool> ReplyMessageAsync(long repliedMessageId, int repliedMessageChatId,
            long newMessageId, int newMessageSenderId, byte[] content, int messageTypeId);
        Task<bool> ForwardMessageAsync(long forwardedMessageId, byte[] content, int messageTypeId, int chatId, int userId);
        Task<bool> EditMessageAsync(long messageId, byte[] content, int messageTypeId);
    }
}
