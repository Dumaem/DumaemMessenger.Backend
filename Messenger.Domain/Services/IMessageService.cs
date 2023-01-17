using Messenger.Domain.Models;

namespace Messenger.Domain.Services
{
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(Chat chat);
        Task<bool> DeleteMessageAsync(Message message, bool isForAll);
        Task<bool> ReadMessageAsync(Message message);
        Task<bool> ReplyMessageAsync(Message message);
        Task<bool> ForwardMessageAsync(Message message, Chat chat);
        Task<bool> EditMessageAsync(Message message);
    }
}
