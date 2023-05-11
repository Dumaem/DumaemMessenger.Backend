namespace Messenger.Domain.Services
{
    public interface IMessageService
    {
        Task SendMessageAsync(int chatId, int userId, byte[] content, int messageTypeId);
        Task DeleteMessageAsync(long messageId, int? userId = null);
        Task ReadMessageAsync(long messageId, int userId);
        Task ReplyMessageAsync(long repliedMessageId, int repliedMessageChatId,
            int newMessageSenderId, byte[] content, int messageTypeId);
        Task ForwardMessageAsync(long forwardedMessageId, byte[] content, int messageTypeId, int chatId, int userId);
        Task EditMessageAsync(long messageId, byte[] content, int messageTypeId);
    }
}
